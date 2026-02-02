using JirHub.Entities.NguyenLPK.Models;
using JirHub.Repositories.NguyenLPK;
using JirHub.Repositories.NguyenLPK.implements;
using JirHub.Services.NguyenLPK.utils;
using Microsoft.EntityFrameworkCore;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JirHub.Services.NguyenLPK.implements
{
    public class GithubService : IGithubService
    {

        private readonly Regex _jiraKeyRegex = new Regex(@"([A-Z]+-\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly GithubRepositoryNguyenLPK _gitHubRepository;
        private readonly GithubPrRepository _gitHubPrRepository;
        private readonly ProjectRepoRepositoryNguyenLPK _projectRepoRepository;
        private readonly GithubPrReviewRepository _githubPrReviewRepository;
        private readonly ProjectConfigRepository _projectConfigRepository;
        private readonly GroupMemberRepository _groupMemberRepository;
        private readonly GitHubCommitRepository _gitHubCommitRepository;
        private readonly WorkLinkRepository _workLinkRepository;
        private readonly GithubIssueRepository _githubIssueRepository;
        public GithubService() 
        {
            _gitHubRepository = new GithubRepositoryNguyenLPK();
            _projectRepoRepository = new ProjectRepoRepositoryNguyenLPK();
            _gitHubPrRepository = new GithubPrRepository();
            _githubPrReviewRepository = new GithubPrReviewRepository();
            _projectConfigRepository = new ProjectConfigRepository();
            _groupMemberRepository = new GroupMemberRepository();
            _gitHubCommitRepository = new GitHubCommitRepository();
            _workLinkRepository = new WorkLinkRepository();
            _githubIssueRepository = new GithubIssueRepository();
        }

        public Task<List<GithubCommitsNguyenLpk>> SearchCommitsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SyncCommitsForRepo(int repoId, string githubToken)
        {
            bool result = false;
            try
            {
                ProjectReposNguyenLpk repo = await _gitHubRepository.GetProjectRepoByIdAsync(repoId);

                if (repo == null)
                {
                    result = false;
                    return result;
                }

                var groupMembers = await _projectRepoRepository.GetGroupMembersByGroupIdAsync(repo.GroupId);

                var existingHashes = await _gitHubRepository.GetExistingCommitHashesAsync(repoId);

                GitHubClient client = new GitHubClient(new ProductHeaderValue("JirHub"));

                client.Credentials = new Credentials(githubToken);

                var repoInfo = GitHubUtils.GetRepoInfoFromUrl(repo.RepoUrl);

                var allCommits = await client.Repository.Commit.GetAll(repoInfo.Value.Owner, repoInfo.Value.Name);

                foreach (var commit in allCommits)
                {
                    if (existingHashes.Contains(commit.Sha))
                    {
                        continue;
                    }
                    
                    var fullCommit = await client.Repository.Commit.Get(repoInfo.Value.Owner, repoInfo.Value.Name, commit.Sha);

                    var commitEntity = ProcessCommitData(fullCommit, repoId, groupMembers);

                    await _gitHubRepository.AddCommitAsync(commitEntity);
                    
                    await ProcessJiraLinks(repo.GroupId, commitEntity.Message, "COMMIT", commitEntity.CommitHash);
                }

                await _gitHubRepository.SaveAsync();
                result = true;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex}");
                result = false;
            }
            

            return result;
            
        }

        private async Task ProcessJiraLinks(int groupId, string content, string type, string id)
        {
            if (string.IsNullOrEmpty(content)) return;
            var matches = _jiraKeyRegex.Matches(content);

            foreach (Match match in matches)
            {
                var key = match.Value.ToUpper();
                if (!await _workLinkRepository.WorkLinkExistsAsync(key, id, type))
                {
                    await _workLinkRepository.AddWorkLinkAsync(new WorkLinksNguyenLpk
                    {
                        GroupId = groupId,
                        JiraIssueKey = key,
                        GithubEntityType = type,
                        GithubEntityId = id,
                        LinkedAt = DateTime.Now
                    });
                }
            }
        }

        public async Task<bool> SyncGroupDataAsync(int groupId)
        {
            bool result = false;
            try
            {
                ProjectConfig projectConfig = await _projectConfigRepository.GetProjectConfigByGroupIdAsync(groupId);
                if (projectConfig == null || string.IsNullOrEmpty(projectConfig.GithubToken)) return result;
                
                List<ProjectReposNguyenLpk> repos = await _projectRepoRepository.GetProjectRepoByGroupIdAsync(groupId);
                List<GroupMember> members = await _groupMemberRepository.GetGroupMemberByGroupId(groupId);

                var client = new GitHubClient(new ProductHeaderValue("JirHub"));
                client.Credentials = new Credentials(projectConfig.GithubToken);

                foreach(ProjectReposNguyenLpk repo in repos)
                {
                    var repoInfo =  GitHubUtils.GetRepoInfoFromUrl(repo.RepoUrl);
                    if (repo.IsActive == false) continue;
                    if (repoInfo == null) continue;
                    await SyncCommitsAsync(client, repo, repoInfo.Value.Owner, repoInfo.Value.Name, members);
                    await SyncPullRequestsAsync(client, repo, repoInfo.Value.Owner, repoInfo.Value.Name, members);
                    await SyncGithubIssuesAsync(client, repo, repoInfo.Value.Owner, repoInfo.Value.Name);
                }
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return result;
            }
            return result;
        }

        private async Task SyncGithubIssuesAsync(GitHubClient client, ProjectReposNguyenLpk repo, string owner, string name)
        {
            try
            {
                var issues = await client.Issue.GetAllForRepository(owner, name);
                foreach (var issue in issues)
                {
                    if (issue.PullRequest != null) continue;

                    bool exists = await _githubIssueRepository.ExistIusse(issue.Number, repo.RepoId);
                    if (!exists)
                    {
                        _githubIssueRepository.CreateAsync(new GithubIssuesNguyenLpk
                        {
                            RepoId = repo.RepoId,
                            IssueNumber = issue.Number,
                            Title = issue.Title,
                            State = issue.State.StringValue,
                            AuthorGithubUsername = issue.User.Login,
                            CreatedAt = issue.CreatedAt.DateTime,
                            UpdatedAt = issue.UpdatedAt?.DateTime
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);            
            }
        }

        private async Task SyncPullRequestsAsync(GitHubClient client, ProjectReposNguyenLpk repo, string owner, string name, List<GroupMember> members)
        {
            PullRequestRequest pullRequestRequest = new PullRequestRequest { State = ItemStateFilter.All };

            IReadOnlyList<PullRequest> pullRequests = await client.PullRequest.GetAllForRepository(owner, name, pullRequestRequest);

            foreach(PullRequest pullRequest in pullRequests)
            {
                GithubPullRequestsNguyenLpk existingPullRequest = await _gitHubPrRepository.ExistPullRequestAsync(pullRequest.Number, repo.RepoId);
                
                if (existingPullRequest == null)
                {
                    existingPullRequest = new GithubPullRequestsNguyenLpk()
                    {
                        RepoId = repo.RepoId,
                        PrNumber = pullRequest.Number,
                        CreatedAt = pullRequest.CreatedAt.DateTime,
                    };
                    await _gitHubPrRepository.CreateAsync(existingPullRequest);
                }
                
                existingPullRequest.Title = pullRequest.Title;
                existingPullRequest.State = pullRequest.State.StringValue;
                existingPullRequest.IsMerged = pullRequest.Merged;
                existingPullRequest.MergedAt = pullRequest.MergedAt?.DateTime;
                existingPullRequest.AuthorGithubUsername = pullRequest.User.Login;
                existingPullRequest.Additions = pullRequest.Additions;
                existingPullRequest.Deletions = pullRequest.Deletions;
                existingPullRequest.ChangedFiles = pullRequest.ChangedFiles;

                GroupMember? member = members.FirstOrDefault(m => m.GithubUsername == existingPullRequest.AuthorGithubUsername);
                if (member != null)
                {
                    existingPullRequest.MappedMemberId = member.MemberId;
                }

                Match jiraMapping = Regex.Match(existingPullRequest.Title, @"([A-Z]+-\d+)", RegexOptions.IgnoreCase);
                if (jiraMapping.Success)
                {
                    existingPullRequest.LinkedIssueKey = jiraMapping.Value.ToUpper();
                }

                IReadOnlyList<PullRequestReview> reviews = await client.PullRequest.Review.GetAll(owner, name, pullRequest.Number);

            }



        }

        private async Task SyncCommitsAsync(GitHubClient client, ProjectReposNguyenLpk repo, string owner, string name, List<GroupMember> members)
        {
            var commits = await client.Repository.Commit.GetAll(owner, name);

            foreach (var commit in commits)
            {
                GithubCommitsNguyenLpk existingCommit = await _gitHubCommitRepository.ExistCommit(commit.Sha, repo.RepoId);

                if (existingCommit == null)
                {
                    var detailedCommit = await client.Repository.Commit.Get(owner, name, commit.Sha);

                    var newCommit = new GithubCommitsNguyenLpk
                    {
                        RepoId = repo.RepoId,
                        CommitHash = detailedCommit.Sha,
                        Message = detailedCommit.Commit.Message,
                        CommittedDate = detailedCommit.Commit.Author.Date.DateTime,
                        Additions = detailedCommit.Stats.Additions, 
                        Deletions = detailedCommit.Stats.Deletions, 
                        AuthorGithubUsername = detailedCommit.Author?.Login ?? detailedCommit.Commit.Author.Name
                    };

                    var member = members.FirstOrDefault(m => m.GithubUsername == newCommit.AuthorGithubUsername);
                    if (member != null) newCommit.MappedMemberId = member.MemberId;

                    var jiraMatch = Regex.Match(newCommit.Message, @"([A-Z]+-\d+)", RegexOptions.IgnoreCase);
                    if (jiraMatch.Success)
                    {
                        newCommit.LinkedIssueKey = jiraMatch.Value.ToUpper();
                    }

                    await _gitHubCommitRepository.CreateAsync(newCommit);

                    CreateWorkLinks(repo.GroupId, newCommit.Message, "COMMIT", newCommit.CommitHash);
                }
            }
        }

        private void CreateWorkLinks(int groupId, string message, string type, string commitHash)
        {
            if (string.IsNullOrEmpty(message)) return;

            MatchCollection matches = _jiraKeyRegex.Matches(message);


            foreach(Match match in matches)
            {
                string key = match.Value.ToUpper();

                bool localExist = _workLinkRepository.ExistWorkLink(key, type, commitHash);

                if (!localExist)
                {
                    _workLinkRepository.CreateAsync(new WorkLinksNguyenLpk
                    {
                        GroupId = groupId,
                        JiraIssueKey = key,
                        GithubEntityType = type,
                        GithubEntityId = commitHash,
                    });
                }

            }

        }

        public async Task<bool> SyncPullRequestsForRepo(int repoId, string githubToken)
        {
            bool result = false;
            try
            {
                var repo = await _gitHubRepository.GetProjectRepoByIdAsync(repoId);
                if (repo == null)
                {
                    result = false;
                    return result;
                }

                var groupMembers = await _projectRepoRepository.GetGroupMembersByGroupIdAsync(repo.RepoId);

                var client = new GitHubClient(new ProductHeaderValue("JirHub"));

                client.Credentials = new Credentials(githubToken);

                string[] segments = repo.RepoUrl.TrimEnd('/').Split('/');
                string owner = segments[segments.Length - 2];
                string name = segments[segments.Length - 1];

                var prRequest = new PullRequestRequest
                {
                    State = ItemStateFilter.All
                };

                var allPullrequests = await client.PullRequest.GetAllForRepository(owner, name, prRequest);


                foreach (var pullRequest in allPullrequests)
                {
                    var existingPr = await _gitHubPrRepository.GetPullRequestByNumberAsync(repoId, pullRequest.Number);

                    int? authorId = null;
                    if (pullRequest.User != null)
                    {
                        var member = groupMembers.FirstOrDefault(m => m.GithubUsername == pullRequest.User.Login);

                        if (member != null) 
                        {
                            authorId = member.MemberId;
                        }

                        string issueKey = null;

                        if (!string.IsNullOrEmpty(pullRequest.Title))
                        {
                            var match = Regex.Match(pullRequest.Title, @"[A-Z]+-\d+");
                            if (match.Success) issueKey = match.Value;
                        }

                        GithubPullRequestsNguyenLpk prEntity;

                        if (existingPr == null)
                        {
                            prEntity = new GithubPullRequestsNguyenLpk
                            {
                                RepoId = repoId,
                                PrNumber = pullRequest.Number,
                                Title = pullRequest.Title,
                                State = pullRequest.State.StringValue,
                                IsMerged = pullRequest.MergedAt.HasValue,
                                AuthorGithubUsername = pullRequest.User?.Login,
                                MappedMemberId = authorId,
                                CreatedAt = pullRequest.CreatedAt.DateTime,
                                MergedAt = pullRequest.MergedAt?.DateTime,
                                ClosedAt = pullRequest.ClosedAt?.DateTime,
                                LinkedIssueKey = issueKey,
                              
                            };
                            await _gitHubPrRepository.AddPullRequestAsync(prEntity);
                            await _gitHubRepository.SaveAsync();
                        } else
                        {
                            prEntity = existingPr;
                            prEntity.State = pullRequest.State.StringValue;
                            prEntity.IsMerged = pullRequest.MergedAt.HasValue;
                            prEntity.MergedAt = pullRequest.MergedAt?.DateTime;
                            prEntity.ClosedAt = pullRequest.ClosedAt?.DateTime;
                            await _gitHubPrRepository.UpdatePullRequestAsync(prEntity);
                        }
                        var reviewsApi = await client.PullRequest.Review.GetAll(owner, name, pullRequest.Number);
                        var reviewsToAdd = new List<GithubPrReviewsNguyenLpk>();

                        foreach (var rev in reviewsApi)
                        {
                           
                            int? reviewerId = null;
                            if (rev.User != null)
                            {
                                var rMember = groupMembers.FirstOrDefault(m => m.GithubUsername == rev.User.Login);
                                if (rMember != null) reviewerId = rMember.MemberId;
                            }

                            if (existingPr != null && existingPr.GithubPrReviewsNguyenLpks.Any(r => r.ReviewerUsername == rev.User.Login && r.State == rev.State.StringValue))
                            {
                                continue; 
                            }

                            reviewsToAdd.Add(new GithubPrReviewsNguyenLpk
                            {
                                PrId = prEntity.PrId,
                                ReviewerUsername = rev.User.Login,
                                MappedReviewerId = reviewerId,
                                State = rev.State.StringValue, 
                                SubmittedAt = rev.SubmittedAt.DateTime
                            });
                        }

                        if (reviewsToAdd.Any())
                        {
                            await _githubPrReviewRepository.AddPrReviewsAsync(reviewsToAdd);
                        }
                    }
                    await _gitHubPrRepository.SaveAsync();
                    result = true;

                }



            } 
            catch (Exception ex)
            {
                Console.WriteLine($"Error Syncing PRs: {ex}");
                result = false;
                return result;
            }
            return result;
        }

        private GithubCommitsNguyenLpk ProcessCommitData(GitHubCommit commit, int repoId, List<GroupMember> members)
        {
            string authorLogin = commit.Author?.Login;
            int? mappedMemberId = null;

            if (!string.IsNullOrEmpty(authorLogin))
            {
                var member = members.FirstOrDefault(m => m.GithubUsername == authorLogin);
                if (member != null) mappedMemberId = member.MemberId;
            }

            string issueKey = null;
            if (!string.IsNullOrEmpty(commit.Commit.Message))
            {
                var match = Regex.Match(commit.Commit.Message, @"[A-Z]+-\d+");
                if (match.Success) issueKey = match.Value;
            }

            return new GithubCommitsNguyenLpk
            {
                RepoId = repoId,
                CommitHash = commit.Sha,
                Message = commit.Commit.Message,
                AuthorGithubUsername = authorLogin ?? "Unknown",
                CommittedDate = commit.Commit.Author.Date.DateTime,
                MappedMemberId = mappedMemberId,
                LinkedIssueKey = issueKey,
                Additions = commit.Stats.Additions,
                Deletions = commit.Stats.Deletions
            };
        }
    }
}

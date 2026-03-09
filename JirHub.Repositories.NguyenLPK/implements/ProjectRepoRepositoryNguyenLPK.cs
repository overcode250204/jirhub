using JirHub.Entities.NguyenLPK.Models;
using JirHub.Repositories.NguyenLPK.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Repositories.NguyenLPK.implements
{
    public class ProjectRepoRepositoryNguyenLPK : GenericRepository<ProjectReposNguyenLpk>
    {
        public ProjectRepoRepositoryNguyenLPK() { }

        public ProjectRepoRepositoryNguyenLPK(prn222Context context) => _context = context;

        public async Task<List<GithubCommitsNguyenLpk>> GetCommitsByRepoId(int repoId)
        {
            return await _context.GithubCommitsNguyenLpks.Where(c => c.RepoId == repoId).OrderByDescending(c => c.CommittedDate).ToListAsync();
        }
        public async Task<List<ProjectReposNguyenLpk>> GetProjectRepoByGroupIdAsync(int groupId)
        {
            return await _context.ProjectReposNguyenLpks.Where(r => r.GroupId == groupId).ToListAsync();
        }

        public async Task<List<ProjectReposNguyenLpk>> GetAllAsync()
        {
            var items =  await _context.ProjectReposNguyenLpks.Include(r => r.GithubCommitsNguyenLpks).ToListAsync();
            return items ?? new List<ProjectReposNguyenLpk>();
        }

        public async Task<ProjectReposNguyenLpk> GetProjectRepoById(int? id)
        {
            return await _context.ProjectReposNguyenLpks.Include(p => p.GithubPullRequestsNguyenLpks).FirstOrDefaultAsync(p => p.RepoId == id);
        }

        public async Task<List<ProjectReposNguyenLpk>> SearchAsync(string repoName, string repoType, string groupName)
        {
            var items = await _context.ProjectReposNguyenLpks
                .Include(p => p.Group)
                .Where(p => (p.RepoName.Contains(repoName) || string.IsNullOrEmpty(repoName)) 
                && (p.RepoType.Contains(repoType) || string.IsNullOrEmpty(repoType))
                && (p.Group.GroupName.Contains(groupName) || string.IsNullOrEmpty(groupName)))
                .ToListAsync();
            return items ?? new List<ProjectReposNguyenLpk>();
        }

        public async Task<List<GroupMember>> GetGroupMembersByGroupIdAsync(int groupId)
        {
            return await _context.GroupMembers.Where(m => m.GroupId == groupId).ToListAsync();
        }

        public async Task<List<int>> GetLeaderGroupIdsAsync(int userId)
        {
            return await _context.GroupMembers
                .Where(m => m.UserId == userId && m.IsLeader == true)
                .Select(m => m.GroupId)
                .ToListAsync();
        }

        public async Task<List<int>> GetStudentGroupIdsAsync(int userId)
        {
            return await _context.GroupMembers
                .Where(m => m.UserId == userId)
                .Select(m => m.GroupId)
                .ToListAsync();
        }

        public async Task<List<int>> GetLecturerGroupIdsAsync(int lecturerId)
        {
            return await _context.ClassGroups
                .Where(g => g.LecturerId == lecturerId)
                .Select(g => g.GroupId)
                .ToListAsync();
        }

        public async Task<List<GithubPullRequestsNguyenLpk>> GetPrsByRepoId(int repoId)
        {
            return await _context.GithubPullRequestsNguyenLpks.Include(p => p.GithubPrReviewsNguyenLpks).Where(p => p.RepoId == repoId).OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task<bool> DeleteProjectRepo(int? repoId)
        {
            bool result = false;
            if (repoId == null)
                return result;

            var repo = await _context.ProjectReposNguyenLpks
                .FirstOrDefaultAsync(r => r.RepoId == repoId);

            if (repo == null)
                return result;

            var pullRequests = await _context.GithubPullRequestsNguyenLpks
                .Where(pr => pr.RepoId == repoId)
                .ToListAsync();

            var prIds = pullRequests.Select(pr => pr.PrId).ToList();

            var reviews = await _context.GithubPrReviewsNguyenLpks
                .Where(r => prIds.Contains(r.PrId))
                .ToListAsync();

            var commits = await _context.GithubCommitsNguyenLpks
                .Where(c => c.RepoId == repoId)
                .ToListAsync();

            _context.GithubPrReviewsNguyenLpks.RemoveRange(reviews);
            _context.GithubPullRequestsNguyenLpks.RemoveRange(pullRequests);
            _context.GithubCommitsNguyenLpks.RemoveRange(commits);
            _context.ProjectReposNguyenLpks.Remove(repo);

            await _context.SaveChangesAsync();
            result = true;
            return result;
        }

        public async Task<int> UpdateRepoAsync(ProjectReposNguyenLpk entity)
        {
            // Dùng AsTracking() cục bộ để bypass `UseQueryTrackingBehavior(NoTracking)` toàn cục
            var existing = await _context.ProjectReposNguyenLpks
                .AsTracking()
                .FirstOrDefaultAsync(r => r.RepoId == entity.RepoId);

            if (existing == null) return 0;

            // Chỉ cập nhật các field cho phép thay đổi, không động vào FK hay navigation
            existing.GroupId  = entity.GroupId;
            existing.RepoName = entity.RepoName;
            existing.RepoUrl  = entity.RepoUrl;
            existing.RepoType = entity.RepoType;
            existing.IsActive = entity.IsActive;

            return await _context.SaveChangesAsync();
        }

        public async Task CreateAsync()
        {
            await CreateAsync();
        }
    }
}

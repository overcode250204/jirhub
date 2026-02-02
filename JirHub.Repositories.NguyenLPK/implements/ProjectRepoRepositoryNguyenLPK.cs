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
            return await _context.ProjectReposNguyenLpks.FindAsync(id);
        }

        public async Task<List<ProjectReposNguyenLpk>> SearchAsync(string repoName, string repoType, string groupName)
        {
            var items = await _context.ProjectReposNguyenLpks
                .Include(p => p.GithubCommitsNguyenLpks)
                .Where(p => (p.RepoName.Contains(repoName) || string.IsNullOrEmpty(repoName)) 
                && (p.RepoType.Contains(repoType) || string.IsNullOrEmpty(repoType))
                && (p.Group.GroupName.Contains(groupName) || string.IsNullOrEmpty(repoType)))
                .ToListAsync();
            return items ?? new List<ProjectReposNguyenLpk>();
        }

        public async Task<List<GroupMember>> GetGroupMembersByGroupIdAsync(int groupId)
        {
            return await _context.GroupMembers.Where(m => m.GroupId == groupId).ToListAsync();
        }

        public async Task<List<GithubPullRequestsNguyenLpk>> GetPrsByRepoId(int repoId)
        {
            return await _context.GithubPullRequestsNguyenLpks.Include(p => p.GithubPrReviewsNguyenLpks).Where(p => p.RepoId == repoId).OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task<bool> DeleteProjectRepo(int? repoId)
        {
            bool result = false;
            ProjectReposNguyenLpk entity = await _context.ProjectReposNguyenLpks.FindAsync(repoId);
            if (entity == null) 
            {
                result = false;
                return result;
            } 
            return await RemoveAsync(entity);
        }

        public async Task CreateAsync()
        {
            await CreateAsync();
        }
    }
}

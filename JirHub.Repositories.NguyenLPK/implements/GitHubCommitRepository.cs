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
    public class GitHubCommitRepository : GenericRepository<GithubCommitsNguyenLpk>
    {

        public GitHubCommitRepository() { }

        public GitHubCommitRepository(prn222Context context) => _context = context;

        public async Task<GithubCommitsNguyenLpk?> ExistCommit(string sha, int repoId)
        {
            return await _context.GithubCommitsNguyenLpks.FirstOrDefaultAsync(c => c.CommitHash == sha && c.RepoId == repoId);
        }

        public async Task<List<GithubCommitsNguyenLpk>> GetAllCommitsAsync()
        {
            return await _context.GithubCommitsNguyenLpks.Include(c => c.MappedMember).Include(c => c.Repo).ToListAsync();
        }

        public async Task<GithubCommitsNguyenLpk> GetCommitByIdAsync(long? id)
        {
            return await _context.GithubCommitsNguyenLpks.Include(c => c.MappedMember).Include(c => c.Repo).FirstOrDefaultAsync(c => c.RepoId == id);
        }

        public async Task<List<GithubCommitsNguyenLpk>> SearchAsync(int repoId, string repoName)
        {
            return await _context.GithubCommitsNguyenLpks.Include(c => c.MappedMember).Where(c => c.RepoId == repoId || c.Repo.RepoName == repoName).OrderByDescending(c => c.CommittedDate).ToListAsync();
        }
    }
}

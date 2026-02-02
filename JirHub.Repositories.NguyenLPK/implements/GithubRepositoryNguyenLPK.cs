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
    public class GithubRepositoryNguyenLPK : GenericRepository<GithubCommitsNguyenLpk>
    {
        public GithubRepositoryNguyenLPK() { }
        public GithubRepositoryNguyenLPK(prn222Context context) => _context ??= context;

        public async Task AddCommitAsync(GithubCommitsNguyenLpk commit)
        {
            await _context.GithubCommitsNguyenLpks.AddAsync(commit);
        }

        public async Task<bool> CommitExistsAsync(string hash)
        {
            return await _context.GithubCommitsNguyenLpks.AnyAsync(c => c.CommitHash == hash);
        }

        

        public async Task<ProjectReposNguyenLpk> GetProjectRepoByIdAsync(int repoId)
        {
            return await _context.ProjectReposNguyenLpks.FindAsync(repoId);
        }
        public async Task<HashSet<string>> GetExistingCommitHashesAsync(int repoId)
        {
            // Chỉ select CommitHash để nhẹ dữ liệu
            var hashes = await _context.GithubCommitsNguyenLpks
                                       .Where(c => c.RepoId == repoId)
                                       .Select(c => c.CommitHash)
                                       .ToListAsync();
            return new HashSet<string>(hashes); // HashSet giúp tìm kiếm O(1) siêu nhanh
        }

        public async Task AddRangeCommitsAsync(IEnumerable<GithubCommitsNguyenLpk> commits)
        {
            await _context.GithubCommitsNguyenLpks.AddRangeAsync(commits);
        }

 
    }
}

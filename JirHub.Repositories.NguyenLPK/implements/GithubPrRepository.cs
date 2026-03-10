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
    public class GithubPrRepository : GenericRepository<GithubPullRequestsNguyenLpk>
    {
        public GithubPrRepository() { }

        public GithubPrRepository(prn222Context context) => _context = context;



        public async Task AddPullRequestAsync(GithubPullRequestsNguyenLpk pr)
        {
            await CreateAsync(pr);
        }

        public async Task<GithubPullRequestsNguyenLpk> ExistPullRequestAsync(int number, int repoId)
        {
            return await _context.GithubPullRequestsNguyenLpks.FirstOrDefaultAsync(p => p.PrNumber == number && p.RepoId == repoId);
        }

        public async Task<List<GithubPullRequestsNguyenLpk>> GetAllPullRequestsAsync()
        {
            return await _context.GithubPullRequestsNguyenLpks.Include(g => g.MappedMember).Include(g => g.Repo).ToListAsync();
        }

        public async Task<GithubPullRequestsNguyenLpk> GetPullRequestByIdAsync(long? id)
        {
            return await _context.GithubPullRequestsNguyenLpks
                .Include(g => g.MappedMember)
                .Include(g => g.Repo)
                .Include(g => g.GithubPrReviewsNguyenLpks)
                .FirstOrDefaultAsync(m => m.PrId == id);
        }

        public async Task<GithubPullRequestsNguyenLpk> GetPullRequestByNumberAsync(int repoId, int prNumber)
        {
            return await _context.Set<GithubPullRequestsNguyenLpk>().Include(p => p.GithubPrReviewsNguyenLpks).FirstOrDefaultAsync(p => p.RepoId == repoId && p.PrNumber == prNumber);
        }

        public async Task<List<GithubPullRequestsNguyenLpk>> SearchPullRequestsAsync(int repoId, string repoName)
        {
            return await _context.GithubPullRequestsNguyenLpks.Include(g => g.Repo).Include(g => g.MappedMember).Where(p => p.RepoId == repoId || p.Repo.RepoName == repoName).OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task UpdatePullRequestAsync(GithubPullRequestsNguyenLpk pr)
        {
            await UpdateAsync(pr);
        }

        public async Task<Dictionary<int, GithubPullRequestsNguyenLpk>> GetExistingPullRequestsDictAsync(int repoId)
        {
            var prs = await _context.GithubPullRequestsNguyenLpks
                .Where(p => p.RepoId == repoId)
                .ToListAsync();
            return prs.ToDictionary(p => p.PrNumber, p => p);
        }
    }
}

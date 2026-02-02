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

        public async Task<GithubPullRequestsNguyenLpk> GetPullRequestByNumberAsync(int repoId, int prNumber)
        {
            return await _context.Set<GithubPullRequestsNguyenLpk>().Include(p => p.GithubPrReviewsNguyenLpks).FirstOrDefaultAsync(p => p.RepoId == repoId && p.PrNumber == prNumber);
        }

        

        public async Task UpdatePullRequestAsync(GithubPullRequestsNguyenLpk pr)
        {
            await UpdateAsync(pr);
        }

    }
}

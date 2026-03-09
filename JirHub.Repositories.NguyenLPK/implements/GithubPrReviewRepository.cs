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
    public class GithubPrReviewRepository : GenericRepository<GithubPrReviewsNguyenLpk>
    {

        public GithubPrReviewRepository() { }

        public GithubPrReviewRepository(prn222Context context) => _context ??= context;

        public async Task AddPrReviewsAsync(List<GithubPrReviewsNguyenLpk> reviews)
        {
            await _context.Set<GithubPrReviewsNguyenLpk>().AddRangeAsync(reviews);
        }

        public async Task<GithubPrReviewsNguyenLpk> ExistsReview(long prId, string login, DateTime dateTime)
        {
            return await _context.GithubPrReviewsNguyenLpks.Where(pr => pr.PrId == prId && pr.ReviewerUsername == login && pr.SubmittedAt == dateTime).FirstOrDefaultAsync();
        }
    }
}

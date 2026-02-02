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
    public class GithubIssueRepository : GenericRepository<GithubIssuesNguyenLpk>
    {
        public GithubIssueRepository()
        {
        }

        public GithubIssueRepository(prn222Context context) => _context = context;

        public async Task<bool> ExistIusse(int number, int repoId)
        {
            return await _context.GithubIssuesNguyenLpks.AnyAsync(issue => issue.IssueNumber == number && issue.RepoId == repoId);
        }
    }
}

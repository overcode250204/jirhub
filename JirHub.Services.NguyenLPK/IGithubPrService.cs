using JirHub.Entities.NguyenLPK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Services.NguyenLPK
{
    public interface IGithubPrService
    {
        Task<List<GithubPullRequestsNguyenLpk>> GetAllPrsAsync();
        Task<GithubPullRequestsNguyenLpk> GetPrByIdAsync(long? id);
        Task<List<GithubPullRequestsNguyenLpk>> SearchAsync(int repoId, string repoName);
    }
}

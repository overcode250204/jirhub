using JirHub.Entities.NguyenLPK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Services.NguyenLPK
{
    public interface IGithubCommitService
    {
        Task<GithubCommitsNguyenLpk> GetCommitByIdAsync(long? id);
        Task<List<GithubCommitsNguyenLpk>> GetAllCommitAsync();
        Task<List<GithubCommitsNguyenLpk>> SearchAsync(int repoId, string repoName);
    }
}

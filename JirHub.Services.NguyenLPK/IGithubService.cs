using JirHub.Entities.NguyenLPK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Services.NguyenLPK
{
    public interface IGithubService
    {
        Task<bool> SyncGroupDataAsync(int groupId);
        Task<bool> SyncCommitsForRepo(int repoId, string githubToken);
        Task<bool> SyncPullRequestsForRepo(int repoId, string githubToken);



        Task<List<GithubCommitsNguyenLpk>> SearchCommitsAsync();
    }
}

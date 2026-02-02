using JirHub.Entities.NguyenLPK.Models;
using JirHub.Repositories.NguyenLPK.implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Services.NguyenLPK.implements
{
    public class GithubPr : IGithubPrService
    {
        private readonly GithubPrRepository _githubPrRepository;
        public GithubPr() 
        {
            _githubPrRepository = new GithubPrRepository();
        }
        public async Task<List<GithubPullRequestsNguyenLpk>> GetAllPrsAsync()
        {
            return await _githubPrRepository.GetAllPullRequestsAsync();
        }

        public Task<GithubPullRequestsNguyenLpk> GetPrByIdAsync(long? id)
        {
            return _githubPrRepository.GetPullRequestByIdAsync(id);
        }

        public async Task<List<GithubPullRequestsNguyenLpk>> SearchAsync(int repoId, string repoName)
        {
            return await _githubPrRepository.SearchPullRequestsAsync(repoId, repoName);
        }
    }
}

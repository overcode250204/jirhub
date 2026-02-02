using JirHub.Entities.NguyenLPK.Models;
using JirHub.Repositories.NguyenLPK.implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Services.NguyenLPK.implements
{
    public class GithubCommitService : IGithubCommitService
    {

        private readonly GitHubCommitRepository _githubCommitRepository;
        public GithubCommitService()
        {
            _githubCommitRepository = new GitHubCommitRepository();
        }

        public async Task<GithubCommitsNguyenLpk> GetCommitByIdAsync(long? id)
        {
            return await _githubCommitRepository.GetCommitByIdAsync(id);
        }

        public async Task<List<GithubCommitsNguyenLpk>> GetAllCommitAsync()
        {
            return await _githubCommitRepository.GetAllCommitsAsync();
        }

        public async Task<List<GithubCommitsNguyenLpk>> SearchAsync(int repoId, string repoName)
        {
            return await _githubCommitRepository.SearchAsync(repoId, repoName);
        }
    }
}

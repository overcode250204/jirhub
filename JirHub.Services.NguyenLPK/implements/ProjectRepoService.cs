using JirHub.Entities.NguyenLPK.Models;
using JirHub.Repositories.NguyenLPK;
using JirHub.Repositories.NguyenLPK.implements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Services.NguyenLPK.implements
{
    public class ProjectRepoService : IProjectRepoService
    {
        private readonly ProjectRepoRepositoryNguyenLPK _projectRepoRepository;


        public ProjectRepoService() { _projectRepoRepository ??= new ProjectRepoRepositoryNguyenLPK(); }



        public async Task<int> CreateProjectRepoAsync(ProjectReposNguyenLpk projectReposNguyenLpk)
        {
            return await _projectRepoRepository.CreateAsync(projectReposNguyenLpk);
        }

        public async Task<bool> DeleteProjectRepoAsync(int? repoId)
        {
            return await _projectRepoRepository.DeleteProjectRepo(repoId);
        }

        public async Task<List<ProjectReposNguyenLpk>> GetAllAsync()
        {
            return await _projectRepoRepository.GetAllAsync();
        }

        public async Task<List<GithubCommitsNguyenLpk>> GetCommitsByRepoId(int repoId)
        {
            return await _projectRepoRepository.GetCommitsByRepoId(repoId);
        }

        public async Task<List<ProjectReposNguyenLpk>> GetProjectRepoByGroupId(int groupId)
        {
            return await _projectRepoRepository.GetProjectRepoByGroupIdAsync(groupId);
        }

        public async Task<ProjectReposNguyenLpk> GetProjectRepoById(int? repoId)
        {
            return await _projectRepoRepository.GetProjectRepoById(repoId);
        }

        public Task<ProjectReposNguyenLpk> GetProjectRepoById(int repoId)
        {
            return _projectRepoRepository.GetProjectRepoById(repoId);
        }

        public async Task<List<GithubPullRequestsNguyenLpk>> GetPrsByRepoId(int repoId)
        {
            return await _projectRepoRepository.GetPrsByRepoId(repoId);
        }

        public Task<List<GithubPullRequestsNguyenLpk>> GetPrsByRepoId(int? repoId)
        {
            if (!repoId.HasValue) return Task.FromResult(new List<GithubPullRequestsNguyenLpk>());
            return GetPrsByRepoId(repoId.Value);
        }

        public async Task<List<ProjectReposNguyenLpk>> SearchProjectRepo(string nameRepo, string repoType, string groupName)
        {
            return await _projectRepoRepository.SearchAsync(nameRepo, repoType, groupName);
        }

        public async Task<int> UpdateProjectRepoAsync(ProjectReposNguyenLpk entity)
        {
            return await _projectRepoRepository.UpdateRepoAsync(entity);
        }

        public async Task<List<int>> GetLeaderGroupIdsAsync(int userId)
        {
            return await _projectRepoRepository.GetLeaderGroupIdsAsync(userId);
        }

        public async Task<List<int>> GetStudentGroupIdsAsync(int userId)
        {
            return await _projectRepoRepository.GetStudentGroupIdsAsync(userId);
        }

        public async Task<List<int>> GetLecturerGroupIdsAsync(int lecturerId)
        {
            return await _projectRepoRepository.GetLecturerGroupIdsAsync(lecturerId);
        }
    }
}

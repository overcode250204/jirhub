using JirHub.Entities.NguyenLPK.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Services.NguyenLPK
{
    public interface IProjectRepoService
    {
        Task<List<ProjectReposNguyenLpk>> GetProjectRepoByGroupId(int groupId);
        Task<List<GithubCommitsNguyenLpk>> GetCommitsByRepoId(int repoId);
        Task<List<GithubPullRequestsNguyenLpk>> GetPrsByRepoId(int? repoId);

        Task<ProjectReposNguyenLpk> GetProjectRepoById(int? repoId);



        Task<bool> DeleteProjectRepoAsync(int? repoId);

        Task<int> UpdateProjectRepoAsync(ProjectReposNguyenLpk entity);

        Task<List<ProjectReposNguyenLpk>> SearchProjectRepo(string nameRepo, string repoType, string groupName);
        Task<int> CreateProjectRepoAsync(ProjectReposNguyenLpk projectReposNguyenLpk);
        Task<List<ProjectReposNguyenLpk>> GetAllAsync();
    }
}

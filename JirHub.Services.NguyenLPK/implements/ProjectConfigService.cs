using JirHub.Entities.NguyenLPK.Models;
using JirHub.Repositories.NguyenLPK.implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Services.NguyenLPK.implements
{
    public class ProjectConfigService : IProjectConfigService
    {

        private readonly ProjectConfigRepository _projectConfigRepository;

        public ProjectConfigService()
        {
            _projectConfigRepository = new ProjectConfigRepository();
        }

        public async Task<ProjectConfig> GetProjectConfigByGroupIdAsync(int groupId)
        {
           return await _projectConfigRepository.GetProjectConfigByGroupIdAsync(groupId);
        }
    }
}

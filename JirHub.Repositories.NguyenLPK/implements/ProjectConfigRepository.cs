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
    public class ProjectConfigRepository : GenericRepository<ProjectConfig>
    {
        public ProjectConfigRepository() { }

        public ProjectConfigRepository(prn222Context context) => _context = context;


        public async Task<ProjectConfig> GetProjectConfigByGroupIdAsync(int groupId)
        {
            return await _context.ProjectConfigs.FirstOrDefaultAsync(p => p.GroupId == groupId);
        }





    }
}

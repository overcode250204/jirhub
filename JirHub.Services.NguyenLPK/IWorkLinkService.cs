using JirHub.Entities.NguyenLPK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Services.NguyenLPK
{
    public interface IWorkLinkService
    {
        Task<List<WorkLinksNguyenLpk>> GetWorkLinksByGroupId(int groupId);
        Task<int> AddWorkLinkAsync(WorkLinksNguyenLpk workLink);
        Task<bool> WorkLinkExistsAsync(string jiraKey, string entityId, string type);



    }
}

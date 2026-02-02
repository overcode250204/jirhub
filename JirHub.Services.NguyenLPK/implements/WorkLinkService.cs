using JirHub.Entities.NguyenLPK.Models;
using JirHub.Repositories.NguyenLPK.implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Services.NguyenLPK.implements
{
    public class WorkLinkService : IWorkLinkService
    {
        private readonly WorkLinkRepository _workLinkRepository;
        public WorkLinkService() 
        {
            _workLinkRepository = new WorkLinkRepository();
        }

        public async Task<int> AddWorkLinkAsync(WorkLinksNguyenLpk workLink)
        {
            return await _workLinkRepository.AddWorkLinkAsync(workLink);
        }

        public async Task<List<WorkLinksNguyenLpk>> GetWorkLinksByGroupId(int groupId)
        {
           return await _workLinkRepository.GetWorkLinksByGroupId(groupId);
        }

        public Task<bool> WorkLinkExistsAsync(string jiraKey, string entityId, string type)
        {
            return _workLinkRepository.WorkLinkExistsAsync(jiraKey, entityId, type);
        }
    }
}

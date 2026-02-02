using JirHub.Entities.NguyenLPK.Models;
using JirHub.Repositories.NguyenLPK.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JirHub.Repositories.NguyenLPK.implements
{
    public class WorkLinkRepository : GenericRepository<WorkLinksNguyenLpk>
    {

        public WorkLinkRepository() { }

        public WorkLinkRepository(prn222Context context) => _context = context;

        public async Task<int> AddWorkLinkAsync(WorkLinksNguyenLpk workLink)
        {
            return await CreateAsync(workLink);
        }

        public bool ExistWorkLink(string key, string type, string commitHash)
        {
            return _context.WorkLinksNguyenLpks.Local.Any(w => w.JiraIssueKey == key && w.GithubEntityType == type && w.GithubEntityId == commitHash);

        }

        public async Task<List<WorkLinksNguyenLpk>> GetWorkLinksByGroupId(int groupId)
        {
            return await _context.WorkLinksNguyenLpks.Where(w => w.GroupId == groupId).OrderByDescending(w => w.LinkedAt)
                .ToListAsync();
        }

        public async Task<bool> WorkLinkExistsAsync(string jiraKey, string entityId, string type)
        {
            return await _context.WorkLinksNguyenLpks.AnyAsync(w => w.JiraIssueKey == jiraKey && w.GithubEntityId == entityId && w.GithubEntityType == type);
        }
    }
}

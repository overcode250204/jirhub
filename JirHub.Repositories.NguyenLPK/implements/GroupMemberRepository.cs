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
    public class GroupMemberRepository : GenericRepository<GroupMember>
    {
        public GroupMemberRepository() { }

        public GroupMemberRepository(prn222Context context) => _context = context;

        public async Task<List<GroupMember>> GetGroupMemberByGroupId(int groupId)
        {
            return await _context.GroupMembers.Where(g => g.GroupId == groupId).ToListAsync();
        }
    }
}

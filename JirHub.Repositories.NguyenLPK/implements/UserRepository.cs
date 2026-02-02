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
    public class UserRepository : GenericRepository<User>
    {

        public UserRepository() { }

        public UserRepository(prn222Context context) => _context = context;

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

    }
}

using JirHub.Entities.NguyenLPK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Services.NguyenLPK
{
    public interface ISystemUserAccountService
    {
        Task<User> GetUserAccountAsync(string username, string password);
    }
}

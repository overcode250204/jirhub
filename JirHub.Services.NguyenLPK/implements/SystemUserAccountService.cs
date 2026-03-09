using JirHub.Entities.NguyenLPK.Models;
using JirHub.Repositories.NguyenLPK.implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Services.NguyenLPK.implements
{
    public class SystemUserAccountService : ISystemUserAccountService
    {
        private readonly SystemUserAccountRepository _systemUserAccountRepository;

        public SystemUserAccountService() => _systemUserAccountRepository = new SystemUserAccountRepository();

        public async Task<User> GetUserAccountAsync(string username, string password)
        {

            try
            {
                return await _systemUserAccountRepository.GetUserAccountAsync(username, password);
            } catch (Exception ex)
            {
                return null;
            }
        }

    }
}

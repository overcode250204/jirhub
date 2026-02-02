using JirHub.Entities.NguyenLPK.Models;
using JirHub.Repositories.NguyenLPK.implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Services.NguyenLPK.implements
{
    public class UserService
    {

        private readonly UserRepository _userRepository;
        public UserService()
        {
            _userRepository ??= new UserRepository();
        }

        public async Task<List<User>> GetAllAsync()
        {
            try
            {
                return await _userRepository.GetAllAsync();
            } catch (Exception ex)
            {
                return new List<User>();
            }
        }

    }
}

using JirHub.Entities.NguyenLPK.Models;
using JirHub.Repositories.NguyenLPK.implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Services.NguyenLPK.implements
{
    public class ClassGroupService : IClassGroupService
    {

        private readonly ClassGroupRepository _classGroupRepository;

        public ClassGroupService()
        {
            _classGroupRepository = new ClassGroupRepository();
        }

        public async Task<List<ClassGroup>> GetAllClassGroupAsync()
        {
            return await _classGroupRepository.GetAllAsync();
        }
    }
}

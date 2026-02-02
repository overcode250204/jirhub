using JirHub.Entities.NguyenLPK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Services.NguyenLPK
{
    public interface IClassGroupService
    {
        Task<List<ClassGroup>> GetAllClassGroupAsync();
    }
}

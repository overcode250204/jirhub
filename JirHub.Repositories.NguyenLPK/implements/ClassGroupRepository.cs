using JirHub.Entities.NguyenLPK.Models;
using JirHub.Repositories.NguyenLPK.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Repositories.NguyenLPK.implements
{
    public class ClassGroupRepository : GenericRepository<ClassGroup>
    {
        public ClassGroupRepository() { }

        public ClassGroupRepository(prn222Context context) => _context = context;




    }
}

using Mock.Code;
using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Domain
{
    public interface IAppUserRepository : IRepositoryBase<AppUser>
    {
        DataGrid GetDataGrid(Pagination pag,string LoginName,string Email);
         void Edit(AppUser userEntity);
    }
}

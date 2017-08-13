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
        dynamic GetFormById(int Id);
        DataGrid GetDataGrid(Pagination pag);
         void Edit(AppUser userEntity);
    }
}

using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock.Code;
using System.Linq.Expressions;

namespace Mock.Domain
{

    public class AppRoleRepository : RepositoryBase<AppRole>, IAppRoleRepository
    {


        public dynamic GetRoleJson()
        {
            var entities = this.IQueryable().Where(u => u.DeleteMark == false && u.IsEnableMark == true).OrderBy(u => u.SortCode).Select(u => new
            {
                id = u.Id,
                text = u.RoleName
            }).ToList();

            return entities;
        }

        public DataGrid GetDataGrid(string search)
        {
            Expression<Func<AppRole, bool>> predicate = u => u.DeleteMark == false
            && (search == "" || u.RoleName.Contains(search))
            && (search == "" || u.Remark.Contains(search));
            var entities = this.IQueryable(predicate).OrderBy(u => u.SortCode).ThenByDescending(r => r.Id).Select(u => new
            {
                u.Id,
                u.RoleName,
                u.SortCode,
                u.Remark,
                u.IsEnableMark
            }).ToList();
            return new DataGrid { rows = entities, total = entities.Count() };
        }
    }
}

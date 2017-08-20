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
        { //Type=1时为按钮，下拉菜单框中为选上级菜单，去除按钮
            var entities = this.IQueryable().Where(u => u.DeleteMark == false).OrderBy(u => u.SortCode).Select(u => new
            {
                id = u.Id,
                text = u.RoleName
            }).ToList();

            return entities;
        }
    }
}

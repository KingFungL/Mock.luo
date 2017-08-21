using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock.Code;

namespace Mock.Domain
{
    /// <summary>
    /// 仓储实现层 AppMenuRepository
    /// </summary>]
    public class AppMenuRepository : RepositoryBase<AppMenu>, IAppMenuRepository
    {
        public DataGrid GetTreeGrid(bool isMemu = false)
        {
            var rows = this.IQueryable(r => r.DeleteMark == false).Select(u => new
            {
                u.Id,
                u.MenuName,
                u.SortCode,
                u.Icon,
                u.LinkUrl,
                u.PId,
                u.State,
            }).ToList();
            return new DataGrid { rows = rows, total = rows.Count() };
        }

        public DataGrid GetTreeGrid()
        {
            var entities = this.IQueryable(u => u.DeleteMark == false).Select(r => new {
                r.Id,
                r.PId,
                r.MenuName,
                r.SortCode,
                r.State,
                r.Target
            }).ToList();

            return new DataGrid { rows = entities, total = entities.Count() };

            throw new NotImplementedException();
        }

        public List<AppMenu> GetUserMenus(int userid)
        {
            return this.IQueryable().ToList().Select(u => new AppMenu
            {
                Id = u.Id,
                PId = u.PId,
                MenuName = u.MenuName,
                Icon = u.Icon,
                LinkUrl = u.LinkUrl,
                SortCode = u.SortCode,
                Target = u.Target,
                State = u.State
            }).ToList();
        }

        public string GetUserMenusTree()
        {
            throw new NotImplementedException();
        }

    }
}

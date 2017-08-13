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
        public DataGrid GetAllPersForTreeGridPer(bool isMemu = false)
        {
            throw new NotImplementedException();
        }

        public DataGrid GetButtons()
        {
            throw new NotImplementedException();
        }

        public List<AppMenu> GetInsertOrUpdateList(Dictionary<string, object> di, string type)
        {
            throw new NotImplementedException();
        }

        public dynamic GetMenuButton(int MenuId)
        {
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
                Target=u.Target,
                State=u.State
            }).ToList();
        }

        public string GetUserMenusTree()
        {
            throw new NotImplementedException();
        }

        public void SetMenuButtons(List<string> btnids, int MenuId)
        {
            throw new NotImplementedException();
        }

        public void SubmitForm(AppMenu entity)
        {
            throw new NotImplementedException();
        }
    }
}

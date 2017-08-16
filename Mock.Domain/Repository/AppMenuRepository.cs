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

        public List<AppMenu> GetInsertOrUpdateList(Dictionary<string, object> di, string type)
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
                Target = u.Target,
                State = u.State
            }).ToList();
        }

        public string GetUserMenusTree()
        {
            throw new NotImplementedException();
        }



        public void SubmitForm(AppMenu entity)
        {
            if (entity.Id == 0)
            {
                entity.Create();
                this.Insert(entity);
            }
            else
            {
                entity.Modify(entity.Id);
                string[] modifystrs = { "PId", "MenuName", "SortCode", "State", "Icon", "LinkUrl", "Target", "LastModifyUserId", "LastModifyTime" };
                this.Update(entity, modifystrs);
            }
        }
        public void DeleteForm(int id)
        {
            AppMenu entity = new AppMenu { Id = id };
            entity.Remove();
            this.Update(entity, "DeleteMark", "DeleteUserId", "DeleteTime");
        }
    }
}

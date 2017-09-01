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
    /// <summary>
    /// 仓储实现层 AppMenuRepository
    /// </summary>]
    public class AppMenuRepository : RepositoryBase<AppMenu>, IAppMenuRepository
    {
        public List<TreeNode> GetFancyTreeGrid()
        {
            List<AppMenu> entities = this.GetAppMenuList(u => true);

            return AppMenu.ConvertFancyTreeNodes(entities);
        }


        public DataGrid GetTreeGrid()
        {
            var entities = this.GetAppMenuList(u => true);
            return new DataGrid { rows = entities, total = entities.Count() };
        }

        public List<AppMenu> GetAppMenuList(Expression<Func<AppMenu, bool>> predicate)
        {
            predicate = predicate.And(r => r.DeleteMark == false);
            return this.IQueryable(predicate).OrderBy(r => r.SortCode).ToList().Select(u => new AppMenu
            {
                Id = u.Id,
                PId = u.PId,
                Name = u.Name,
                Icon = u.Icon,
                LinkUrl = u.LinkUrl,
                SortCode = u.SortCode,
                Target = u.Target,
                Folder = u.Folder,
                Expanded = u.Expanded
            }).ToList();
        }

        public string GetUserMenusTree()
        {
            throw new NotImplementedException();
        }
        public List<TreeSelectModel> GetTreeJson()
        {
            //Type=1时为按钮，下拉菜单框中为选上级菜单，去除按钮
            List<TreeSelectModel> treeList = this.IQueryable().Where(u => u.DeleteMark == false).OrderBy(r => r.SortCode).ToList()
                                    .Select(u => new TreeSelectModel
                                    {
                                        id = u.Id.ToString(),
                                        text = u.Name,
                                        parentId = u.PId.ToString()
                                    }).ToList();

            treeList.Insert(0, new TreeSelectModel { id ="-1", text = "==请选择==", parentId = "0" });
            return treeList;
        }
    }
}

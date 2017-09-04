using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock.Code;
using System.Linq.Expressions;
using System.Data.Common;
using System.Data.SqlClient;

namespace Mock.Domain
{
    /// <summary>
    /// 仓储实现层 AppModuleRepository
    /// </summary>]
    public class AppModuleRepository : RepositoryBase<AppModule>, IAppModuleRepository
    {
        public List<TreeNode> GetFancyTreeGrid()
        {
            List<AppModule> entities = this.GetAppModuleList(u => true);

            return AppModule.ConvertFancyTreeNodes(entities);
        }


        public DataGrid GetTreeGrid()
        {
            var entities = this.GetAppModuleList(u => true);
            return new DataGrid { rows = entities, total = entities.Count() };
        }

        /// <summary>
        /// 根据条件得到菜单表数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<AppModule> GetAppModuleList(Expression<Func<AppModule, bool>> predicate)
        {
            predicate = predicate.And(r => r.DeleteMark == false);
            return this.IQueryable(predicate).OrderBy(r => r.SortCode).ToList().Select(u => new AppModule
            {
                Id = u.Id,
                PId = u.PId,
                Name = u.Name,
                Icon = u.Icon,
                LinkUrl = u.LinkUrl,
                SortCode = u.SortCode,
                Target = u.Target,
                Folder = u.Folder,
                Expanded = u.Expanded,
                TypeCode = u.TypeCode
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

            treeList.Insert(0, new TreeSelectModel { id = "-1", text = "==请选择==", parentId = "0" });
            return treeList;
        }

        public List<TreeGridModel> GetButtonTreeJson(int Id)
        {
            //这个sql语句能够找到PId下的所有子节点数据,正好解决递归查询的问题
            //Id,PId,Name,SortCode,EnCode,LinkUrl
            //a.Id,a.PId,a.Name,a.SortCode,a.EnCode,a.LinkUrl
            string sql = @"
WITH TEMP AS 
(SELECT *
      FROM 
        AppModule  
      WHERE 
        PId = 2 
    UNION ALL 
      SELECT a.*
      FROM 
        TEMP  JOIN AppModule a ON TEMP.Id= a.PId AND a.DeleteMark='false'
 )  
SELECT * FROM TEMP ORDER BY SortCode";
            DbParameter[] parameter = new SqlParameter[] {
                new SqlParameter("Id",Id)
            };
            var dglist = this.FindList(sql, parameter);

            var treeList = new List<TreeGridModel>();
            foreach (var item in dglist)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = dglist.Count(t => t.PId == item.Id) == 0 ? false : true;
                treeModel.id = item.Id.ToString();
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = Id == item.PId ? "0" : item.PId.ToString();//发现不这么做，无法转成树结构。走你。
                treeModel.expanded = hasChildren;
                treeModel.entityJson = JsonHelper.SerializeObject(new {item.Id,item.PId, item.Name,item.SortCode,item.EnCode,item.LinkUrl});
                treeList.Add(treeModel);
            }
            return treeList;
        }
    }
}

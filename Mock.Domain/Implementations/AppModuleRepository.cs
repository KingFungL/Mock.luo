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

        /// <summary>
        /// FancyTree插件TreeGrid列表数据
        /// </summary>
        /// <returns></returns>
        public List<TreeNode> GetFancyTreeGrid()
        {
            List<AppModule> entities = this.GetAppModuleList(u => true);

            return AppModule.ConvertFancyTreeNodes(entities);
        }

        /// <summary>
        /// 得到树形treegrid菜单列表数据
        /// </summary>
        /// <returns></returns>
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
                Expanded = u.Expanded,
                TypeCode = u.TypeCode
            }).ToList();
        }

        public string GetUserMenusTree()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 下拉树json数据
        /// </summary>
        /// <returns></returns>
        public List<TreeSelectModel> GetTreeJson(int PId)
        {
            //Type=1时为按钮，下拉菜单框中为选上级菜单，去除按钮
            List<TreeSelectModel> treeList = this.IQueryable().Where(u => u.DeleteMark == false && (PId == 0 || u.PId == PId)).OrderBy(r => r.SortCode).ToList()
                                    .Select(u => new TreeSelectModel
                                    {
                                        id = u.Id.ToString(),
                                        text = u.Name,
                                        parentId = u.PId.ToString()
                                    }).ToList();

            treeList.Insert(0, new TreeSelectModel { id = "-1", text = "==请选择==", parentId = "0" });
            return treeList;
        }

        /// <summary>
        /// 子节点list结构
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<AppModule> GetListJson(int Id)
        {
            return this.GetModuleChildrenList(Id);
        }

        /// <summary>
        ///根据id找到子节点
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private List<AppModule> GetModuleChildrenList(int Id)
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
        PId = @Id AND DeleteMark='false'
    UNION ALL 
      SELECT a.*
      FROM 
        TEMP  JOIN AppModule a ON TEMP.Id= a.PId AND a.DeleteMark='false'
 )  
SELECT * FROM TEMP ORDER BY SortCode";
            DbParameter[] parameter = new SqlParameter[] {
                new SqlParameter("Id",Id)
            };
            List<AppModule> dglist = this.FindList(sql, parameter);
            return dglist;
        }

        /// <summary>
        /// 得到按钮权限树形数据
        /// </summary>
        /// <returns></returns>
        public List<TreeGridModel> GetButtonTreeJson(int Id)
        {
            var dglist = this.GetModuleChildrenList(Id);
            var treeList = new List<TreeGridModel>();
            foreach (var item in dglist)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = dglist.Count(t => t.PId == item.Id) == 0 ? false : true;
                treeModel.id = item.Id.ToString();
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.PId.ToString();
                treeModel.expanded = hasChildren;
                treeModel.entityJson = JsonHelper.SerializeObject(new { item.Id, item.PId, item.Name, item.SortCode, item.EnCode, item.LinkUrl, item.TypeCode });
                treeList.Add(treeModel);
            }
            return treeList;
        }
        /// <summary>
        /// 保存菜单信息并且配置菜单下的按钮
        /// </summary>
        /// <param name="menu">菜单数据</param>
        /// <param name="buttonList">按钮List</param>
        /// <param name="Id">菜单主键</param>
        public void SubmitForm(AppModule module, List<AppModule> buttonList, int Id)
        {
            //前台自动生成了一个小于0的Id，
            if (Id <=0)
            {
                using (var db = new RepositoryBase().BeginTrans())
                {
                    module.Create();
                    db.Insert(module);
                    foreach (var item in buttonList)
                    {
                        item.Create();
                        db.Insert(item);
                    }
                    db.Commit();
                }
            }
            else
            {
                using (var db = new RepositoryBase().BeginTrans())
                {
                    module.Modify(module.Id);
                    db.Update(module, "PId", "EnCode", "Name", "Expanded", "Icon", "Target", "SortCode", "LinkUrl", "TypeCode", "LastModifyUserId", "LastModifyTime");
                    foreach (var item in buttonList)
                    {
                        if (item.Id<=0)
                        {
                            item.Create();
                            db.Insert(item);
                        }
                        else
                        {
                            item.Modify(item.Id);
                            string[] modifyList = { "PId", "EnCode", "Name", "SortCode", "LinkUrl", "TypeCode", "LastModifyUserId", "LastModifyTime" };
                            db.Update(item, modifyList);
                        }
                    }
                    List<AppModule> childrenButtonList = this.GetModuleChildrenList(Id);

                    //存在删除行为
                    if (childrenButtonList.Count != buttonList.Count)
                    {
                        //找到要删除的按钮
                        foreach(var item in childrenButtonList)
                        {
                            int i = 0;
                            int count = buttonList.Count;
                            for ( i = 0; i < count; i++)
                            {
                                if (item.Id == buttonList[i].Id) break;
                            }
                            /*如果没有从前台传来的数据集合中找到id,说明此id，应该被删除，会有外键关联,所以还是置标志位*/
                            if (i ==count)
                            {
                               // db.Delete(new AppModule { Id = item.Id });
                                var deleteEntity = new AppModule { Id = item.Id };
                                deleteEntity.Remove();
                                db.Update(deleteEntity, "DeleteMark", "DeleteUserId", "DeleteTime");
                              
                            }
                        }
                    }
                    db.Commit();

                }
            }
        }

        /// <summary>
        /// 根据角色id获取分配权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public dynamic GetRoleModuleAuth(int roleId)
        {
            var moduleList = this.IQueryable().Where(u=>u.DeleteMark == false).OrderBy(u=>u.SortCode)
               .Select(u => new
               {
                   id = u.Id,
                   pId = u.PId,
                   name = u.Name,
                   u.Icon,
                   @checked = u.RoleModules.Where(r => r.RoleId == roleId && r.ModuleId == u.Id).Count() > 0 ? true : false,
                   open = true
               }).ToList();

            return moduleList;
        }
    }
}

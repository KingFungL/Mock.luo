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
using Mock.Code.Helper;

namespace Mock.Domain
{
    /// <summary>
    /// 仓储实现层 AppModuleRepository
    /// </summary>]
    public class AppModuleRepository : RepositoryBase<AppModule>, IAppModuleRepository
    {
        #region  Constructor
        private readonly IRedisHelper _iRedisHelper;

        public AppModuleRepository(IRedisHelper _iRedisHelper)
        {
            this._iRedisHelper = _iRedisHelper;
        } 
        #endregion


        #region FancyTree插件TreeGrid列表数据
        public List<TreeNode> GetFancyTreeGrid()
        {
            List<AppModule> entities = this.GetAppModuleList(u => true);

            return AppModule.ConvertFancyTreeNodes(entities);
        }
        #endregion

        #region 得到树形treegrid菜单列表数据
        public DataGrid GetTreeGrid()
        {
            var entities = this.GetAppModuleList(u => true);
            return new DataGrid { rows = entities, total = entities.Count() };
        }
        #endregion

        #region  根据条件得到模块表数据

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
                TypeCode = u.TypeCode,
                EnCode = u.EnCode
            }).ToList();
        }
        #endregion

        #region 下拉树json数据
        /// <summary>
        /// 下拉树json数据
        /// </summary>
        /// <returns></returns>
        public List<TreeSelectModel> GetTreeJson(int PId)
        {
            //Type=1时为按钮，下拉菜单框中为选上级菜单，去除按钮
            List<TreeSelectModel> treeList = this.IQueryable(u => u.DeleteMark == false && (PId == 0 || u.PId == PId)).OrderBy(r => r.SortCode).ToList()
                                    .Select(u => new TreeSelectModel
                                    {
                                        id = u.Id.ToString(),
                                        text = u.Name,
                                        parentId = u.PId.ToString()
                                    }).ToList();

            treeList.Insert(0, new TreeSelectModel { id = "-1", text = "==请选择==", parentId = "0" });
            return treeList;
        }
        #endregion

        #region 子节点list结构
        /// <summary>
        /// 子节点list结构
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<AppModule> GetListJson(int Id)
        {
            return this.GetModuleChildrenList(Id);
        }
        #endregion

        #region 根据id找到子节点
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
        #endregion

        #region  得到按钮与权限的树形数据
        /// <summary>
        /// 得到按钮权限树形数据
        /// </summary>
        /// <returns></returns>
        public List<TreeGridModel> GetButtonTreeJson(int Id)
        {
            List<AppModule> dglist = this.GetModuleChildrenList(Id).Where(u => u.TypeCode == ModuleCode.Button.ToString() || u.TypeCode == ModuleCode.Permission.ToString()).ToList();
            var treeList = new List<TreeGridModel>();
            foreach (var item in dglist)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = dglist.Count(t => t.PId == item.Id) == 0 ? false : true;
                treeModel.id = item.Id.ToString();
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.PId.ToString();
                treeModel.expanded = hasChildren;
                treeModel.entityJson = JsonHelper.SerializeObject(new { item.Id, item.PId, item.Icon, item.Name, item.SortCode, item.EnCode, item.LinkUrl, item.TypeCode });
                treeList.Add(treeModel);
            }
            return treeList;
        }
        #endregion

        #region 保存菜单信息并且配置菜单下的按钮
        /// <summary>
        /// 保存菜单信息并且配置菜单下的按钮
        /// </summary>
        /// <param name="menu">菜单数据</param>
        /// <param name="buttonList">按钮List</param>
        /// <param name="Id">菜单主键</param>
        public void SubmitForm(AppModule module, List<AppModule> buttonList, int Id)
        {
            List<AppModule> modulePIdList = new List<AppModule>();
            //源数据,从按钮及权限下通过深copy
            List<int?> moduleSourceIdList = buttonList.Select(u => u.Id).ToList();
            IRepositoryBase db = new RepositoryBase().BeginTrans();
            //前台自动生成了一个小于0的Id
            if (Id <= 0)
            {
                module.Create();
                db.Insert(module);
                foreach (var item in buttonList)
                {
                    item.Create();
                    //说明
                    if (item.PId < 0)
                    {
                        modulePIdList.Add(item);
                    }
                    db.Insert(item);
                }
                db.Commit();

            }
            else
            {
                module.Modify(module.Id);
                db.Update(module, "PId", "EnCode", "Name", "Expanded", "Icon", "Target", "SortCode", "LinkUrl", "TypeCode", "LastModifyUserId", "LastModifyTime");
                foreach (var item in buttonList)
                {
                    if (item.Id <= 0)
                    {
                        item.Create();
                        if (item.PId < 0)
                        {
                            modulePIdList.Add(item);
                        }
                        db.Insert(item);
                    }
                    else
                    {
                        item.Modify(item.Id);
                        string[] modifyList = { "PId", "EnCode", "Icon", "Name", "SortCode", "LinkUrl", "TypeCode", "LastModifyUserId", "LastModifyTime" };
                        db.Update(item, modifyList);
                    }
                }
                List<AppModule> childrenButtonList = this.GetModuleChildrenList(Id);

                //存在删除行为
                if (childrenButtonList.Count != buttonList.Count)
                {
                    //找到要删除的按钮
                    foreach (var item in childrenButtonList)
                    {
                        int i = 0;
                        int count = buttonList.Count;
                        for (i = 0; i < count; i++)
                        {
                            if (item.Id == buttonList[i].Id) break;
                        }
                        /*如果没有从前台传来的数据集合中找到id,说明此id，应该被删除，会有外键关联,所以还是置标志位*/
                        if (i == count)
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
            //这些都是PID为负数的，要将他变成正常的PID节点，因为前端生成的有问题。是模拟的。。哈。
            if (modulePIdList.Count > 0)
            {
                int i = 0;
                foreach (var item in modulePIdList)
                {
                    /*算出item中的PId原本是哪一条记录的子节点。求出他的下标，根据下标,
                     * 再将加入数据库的buttoList[下标]的Id作为modulePIdList[i]下的PId
                     */
                    int count = 0;
                    foreach (int? id in moduleSourceIdList)
                    {
                        if (item.PId == id) break;
                        count++;
                    }
                    if (count < moduleSourceIdList.Count)
                    {
                        modulePIdList[i].PId = buttonList[count].Id;
                    }
                    i++;
                }
                var baseDB = new RepositoryBase().BeginTrans();
                {
                    string[] modiystr = { "PId" };
                    foreach (var item in modulePIdList)
                    {
                        baseDB.Update(item, modiystr);
                    }
                    baseDB.Commit();
                }
            }
        }
        #endregion

        #region 根据角色id获取分配权限
        /// <summary> 
        /// 根据角色id获取分配权限
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns></returns>
        public dynamic GetRoleModuleAuth(int roleId)
        {
            var moduleList = this.IQueryable(u => u.DeleteMark == false).OrderBy(u => u.SortCode)
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
        #endregion

        #region 根据用户ID得到权限模块信息 
        public List<AppModule> GetUserModules(int? userId)
        {
            return _iRedisHelper.UnitOfWork(string.Format(ConstHelper.AppModule,
             "AuthorizeUrl_" + userId), () =>
             {
                 if (OperatorProvider.Provider.CurrentUser.IsAdmin)
                 {
                     return this.IQueryable(r => r.DeleteMark == false).ToList();
                 }
                 //1.根据用户编号得到角色编号(集合)
                 List<int> roleIdList = this.Db.Set<UserRole>().Where(u => u.UserId == userId).Select(u => u.RoleId).ToList();
                 //2.根据角色ID取出菜单编号
                 List<int> menuIdList = this.Db.Set<RoleModule>().Where(u => roleIdList.Contains(u.RoleId)).Select(u => u.ModuleId).ToList();

                 //根据菜单编号得到菜单的具体信息
                 List<AppModule> listModules = this.IQueryable(p => (menuIdList.Contains((int)p.Id))).ToList();

                 return listModules;
             });
        }
        #endregion

        /// <summary>
        /// Action执行权限认证
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="action">请求地址</param>
        /// <returns></returns>
        public bool ActionAuthorize(int userId, string moduleId, string action)
        {
            //这个模块id，需要前台存储cookies，每次切换tab都需要更新cookie
            List<AppModule> authorizeUrlList= this.GetUserModules(userId);
            int imoduleId = 0;
            int.TryParse(moduleId, out imoduleId);
            if (imoduleId == 0)
            {
                LogFactory.GetLogger("权限认证").Info("日志记录：用户为:"+userId+ " action:" + action);
            }
            authorizeUrlList = authorizeUrlList.FindAll(t => t.PId.Equals(imoduleId) || imoduleId == 0);
            foreach (var item in authorizeUrlList)
            {
                if (!string.IsNullOrEmpty(item.LinkUrl))
                {
                    string[] url = item.LinkUrl.Split('?');
                    if (url[0] == action)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

using Mock.Code;
using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Domain
{
    public interface IAppModuleRepository : IRepositoryBase<AppModule>
    {

        /// <summary>
        /// 根据用户ID得到菜单权限 GetUserMenus
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns>用户菜单List</returns>
        List<AppModule> GetAppModuleList(Expression<Func<AppModule,bool>>predicate);

        /// <summary>
        /// 从session中取出当前登录用户对应的菜单并转成递归结构 GetUserMenusTree
        /// </summary>
        /// <returns>用户菜单JsonTree</returns>
        string GetUserMenusTree();
        /// <summary>
        /// 得到树形treegrid菜单列表数据
        /// </summary>
        /// <returns></returns>
        DataGrid GetTreeGrid();

        /// <summary>
        /// FancyTree插件TreeGrid列表数据
        /// </summary>
        /// <returns></returns>
        List<TreeNode> GetFancyTreeGrid();
        /// <summary>
        /// 下拉树json数据
        /// </summary>
        /// <returns></returns>

        List<TreeSelectModel> GetTreeJson();
        /// <summary>
        /// 得到按钮权限树形数据
        /// </summary>
        /// <returns></returns>
        List<TreeGridModel> GetButtonTreeJson(int Id);
    }
}

using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Domain
{
    public interface IAppMenuRepository : IRepositoryBase<AppMenu>
    {

        #region 根据用户ID得到菜单权限 GetUserMenus
        /// <summary>
        /// 根据用户ID得到菜单权限 GetUserMenus
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns>用户菜单List</returns>
        List<AppMenu> GetUserMenus(int userid);
        #endregion

        #region 从session中取出当前登录用户对应的菜单并转成递归结构 GetUserMenusTree
        /// <summary>
        /// 从session中取出当前登录用户对应的菜单并转成递归结构 GetUserMenusTree
        /// </summary>
        /// <returns>用户菜单JsonTree</returns>
        string GetUserMenusTree();
        #endregion

        DataGrid GetTreeGrid(bool isMemu = false);

    
        void SubmitForm(AppMenu entity);

        List<AppMenu> GetInsertOrUpdateList(Dictionary<string, object> di, string type);

        void DeleteForm(int id);

    }
}

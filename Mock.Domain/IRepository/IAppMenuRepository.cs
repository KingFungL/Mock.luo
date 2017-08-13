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

        #region 得到所有权限的TreeGrid模板 GetAllPersForTreeGridPer
        DataGrid GetAllPersForTreeGridPer(bool isMemu = false);
        void SetMenuButtons(List<string> btnids, int MenuId);
        DataGrid GetButtons();
        dynamic GetMenuButton(int MenuId);
        #endregion

        #region SubmitForm  编辑菜单
        void SubmitForm(AppMenu entity);
        #endregion

        List<AppMenu> GetInsertOrUpdateList(Dictionary<string, object> di, string type);

    }
}

using Mock.Code;
using Mock.Data;
using Mock.Data.Models;
using Mock.Domain;
using Mock.Luo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mock.Luo.Areas.Plat.Controllers
{
    public class AppMenuController : BaseController
    {
        // GET: Plat/AppMenu

        private readonly IAppMenuRepository _service;
        public AppMenuController(IAppMenuRepository service)
        {
            this._service = service;
        }

        #region 根据用户id得到用户菜单权限
        public ActionResult GetUserMenu(int userid = 1)
        {
            List<AppMenu> userMenuEntities = _service.GetUserMenus(userid);

            List<TreeNode> treeNodes = AppMenu.ConvertTreeNodes(userMenuEntities);

            return Content(treeNodes.ToJson());

        }
        #endregion

        public ActionResult GetTreeGrid()
        {
            DataGrid dg = _service.GetTreeGrid();

            return Content(dg.ToJson());
        }
    }
}
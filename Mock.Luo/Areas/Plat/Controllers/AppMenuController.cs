using Autofac;
using Mock.Code;
using Mock.Data;
using Mock.Data.Models;
using Mock.Domain;
using Mock.Luo.Areas.Plat.Models;
using Mock.Luo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mock.Luo.Areas.Plat.Controllers
{
    public class AppMenuController : CrudController<AppMenu, AppMenuViewModel>
    {
        // GET: Plat/AppMenu

        private readonly IAppMenuRepository _service;
        public AppMenuController(IAppMenuRepository service, IComponentContext container) : base(container)
        {
            this._service = service;
        }

        #region 根据用户id得到用户菜单权限
        public ActionResult GetUserMenu(int userid = 1)
        {
            List<AppMenu> userMenuEntities = _service.GetAppMenuList(u=>true);

            List<TreeNode> treeNodes = AppMenu.ConvertTreeNodes(userMenuEntities);

            return Content(treeNodes.ToJson());

        }
        #endregion

        public ActionResult GetTreeGrid()
        {
            DataGrid dg = _service.GetTreeGrid();

            return Content(dg.ToJson());
        }

        public ActionResult DemoTreeGrid()
        {
            return View();
        }

        public ActionResult GetFancyTreeGrid()
        {
            return Result(_service.GetFancyTreeGrid());
        }
      
        public ActionResult GetTreeJson()
        {
            List<TreeSelectModel> treeList = _service.GetTreeJson();
            return Content(treeList.TreeSelectJson());
        }

        public ActionResult GetComboBoxTreeJson()
        {
            List<TreeSelectModel> treeList = _service.GetTreeJson();
            return Content(treeList.ComboboxTreeJson());
        }
    }
}
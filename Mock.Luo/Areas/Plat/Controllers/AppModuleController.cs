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
    public class AppModuleController : CrudController<AppModule, AppModuleViewModel>
    {
        // GET: Plat/AppModule
        private readonly IAppModuleRepository _service;
        public AppModuleController(IAppModuleRepository service, IComponentContext container) : base(container)
        {
            this._service = service;
        }

        #region 根据用户id得到用户菜单权限
        public ActionResult GetUserModule(int userid = 1)
        {
            List<AppModule> userModuleEntities = _service.GetAppModuleList(u => u.TypeCode != "Button" && u.TypeCode != "Permission");

            List<TreeNode> treeNodes = AppModule.ConvertTreeNodes(userModuleEntities);

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


        public ActionResult GetComboBoxTreeJson(int PId = 0)
        {
            List<TreeSelectModel> treeList = _service.GetTreeJson(PId);
            return Content(treeList.ComboboxTreeJson(PId));
        }

        public ActionResult GetListJson(int Id)
        {
            return Result(_service.GetListJson(Id));
        }

        public ActionResult GetButtonTreeJson(int Id)
        {
            return Content(_service.GetButtonTreeJson(Id).TreeGridJson(Id));
        }

        public ActionResult ButtonList()
        {
            return View();
        }
        public ActionResult Icon()
        {
            return View();
        }
        public ActionResult Button()
        {
            return View();
        }

        /// <summary>
        /// 将button数组转成前台jqGrid对应的树形表格
        /// </summary>
        /// <returns></returns>
        public ActionResult ToButtonTreeJson(string moduleButtonJson, int Id = 0)
        {
            List<AppModuleViewModel> dglist = JsonHelper.DeserializeJsonToList<AppModuleViewModel>(moduleButtonJson);

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
            return Content(treeList.TreeGridJson(Id));
        }

        public ActionResult SubmitForm(AppModule viewModel, string buttonJson, int Id = 0)
        {
            List<AppModule> buttonList = JsonHelper.DeserializeJsonToList<AppModule>(buttonJson);

            _service.SubmitForm(viewModel,buttonList, Id);

            return Success();
        }
    }
}
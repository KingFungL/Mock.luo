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

        #region 根据当前登录的用户得到用户菜单权限
        public ActionResult GetUserModule()
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

        /// <summary>
        /// 下拉菜单树结构
        /// </summary>
        /// <param name="PId">父节点</param>
        /// <returns></returns>
        public ActionResult GetComboBoxTreeJson(int PId = 0)
        {
            List<TreeSelectModel> treeList = _service.GetTreeJson(PId);
            return Content(treeList.ComboboxTreeJson(PId));
        }

        /// <summary>
        /// 系统按钮列表数据
        /// </summary>
        /// <param name="Id">父ID</param>
        /// <returns></returns>
        public ActionResult GetListJson(int Id)
        {
            return Result(_service.GetListJson(Id));
        }
        /// <summary>
        /// 按钮树形jqGrid数据
        /// </summary>
        /// <param name="Id">父ID</param>
        /// <returns></returns>
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
            List<AppModuleViewModel> dglist = JsonHelper.DeserializeJsonToList<AppModuleViewModel>(moduleButtonJson).OrderBy(u => u.SortCode).ToList();

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
            return Content(treeList.TreeGridJson(Id));
        }

        /// <summary>
        /// 系统功能，系统按钮，权限，一键提交
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="buttonJson"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult SubmitForm(AppModule viewModel, string buttonJson, int Id = 0)
        {
            List<AppModule> buttonList = JsonHelper.DeserializeJsonToList<AppModule>(buttonJson);

            _service.SubmitForm(viewModel, buttonList, Id);

            return Success();
        }
        /// <summary>
        /// 系统按钮上级节点，将列表中的数据实时转成下拉树的json数据
        /// </summary>
        /// <param name="moduleButtonJson"></param>
        /// <returns></returns>
        public ActionResult ListToTreeJson(string moduleButtonJson, int Id = 0)
        {
            List<AppModuleViewModel> dglist = JsonHelper.DeserializeJsonToList<AppModuleViewModel>(moduleButtonJson);

            List<TreeSelectModel> treeList = new List<TreeSelectModel>();
            foreach (var item in dglist)
            {
                treeList.Add(new TreeSelectModel
                {
                    id = item.Id.ToString(),
                    text = item.Name,
                    parentId = item.PId.ToString()
                });
            }

            treeList.Insert(0, new TreeSelectModel { id = "-1", text = "==请选择==", parentId = Convert.ToString(Id) });

            return Content(treeList.ComboboxTreeJson(Id));
        }
        /// <summary>
        /// 根据角色id获取分配权限
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns></returns>
        public ActionResult GetRoleModuleAuth(int roleId)
        {
            return Result(_service.GetRoleModuleAuth(roleId));
        }
    }
}

using Mock.Code;
using Mock.Data;
using Mock.Data.Models;
using Mock.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mock.Luo.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IRedisHelper iRedisHelper;
        private readonly IItemsDetailRepository _itemsDetailSevice;
        private readonly IItemsRepository _itemsService;
        private readonly IAppModuleRepository _appModuleService;
        public HomeController(IRedisHelper iRedisHelper, IItemsDetailRepository _itemsDetailSevice, IItemsRepository _itemsService, IAppModuleRepository _appModuleService)
        {
            this.iRedisHelper = iRedisHelper;
            this._itemsDetailSevice = _itemsDetailSevice;
            this._itemsService = _itemsService;
            this._appModuleService = _appModuleService;
        }
        public ActionResult MainView()
        {
            return View();
        }

        public ActionResult DatalistView()
        {
            return View();
        }

        public ActionResult BlogView()
        {
            //iRedisHelper.StringSet<string>("key", "我是罗志强，这是一个testredis。net版");

            //string value = iRedisHelper.StringGet<string>("key");

            return View();
        }

        public ActionResult TestView()
        {
            return View();
        }

        public ActionResult GetTreeJson()
        {

            return View();
        }

        /// <summary>
        /// 获取当前登录用户的通用数据，如登录用户的菜单权限，及对应的按钮权限，还有一些字典数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetClientsJson()
        {

            var data = new
            {
                dataItems = this.GetDataItemList(),
                user = "",
                authorizeMenu = this.GetMenuList(),
                //authorizeButton = this.GetMenuButtonList(),
            };
            return Content(data.ToJson());
        }
        /// <summary>
        /// 字典键值对
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetDataItemList()
        {
            List<ItemsDetail> itemdata = _itemsDetailSevice.GetList();
            Dictionary<string, object> dictionaryItem = new Dictionary<string, object>();
            List<Items> itemsList = _itemsService.IQueryable(u => u.DeleteMark == false).ToList();
            foreach (Items item in itemsList)
            {
                List<ItemsDetail> dataItemList = itemdata.FindAll(t => t.FId == item.Id);
                Dictionary<string, string> dictionaryItemList = new Dictionary<string, string>();
                foreach (ItemsDetail itemList in dataItemList)
                {
                    dictionaryItemList.Add(itemList.ItemCode, itemList.ItemName);
                }
                dictionaryItem.Add(item.EnCode, dictionaryItemList);
            }
            return dictionaryItem;
        }
        /// <summary>
        /// 当前登录用户的菜单权限
        /// </summary>
        /// <returns>List<TreeNode>树形递归数据</returns>
        private List<TreeNode> GetMenuList()
        {
            OperatorProvider op = OperatorProvider.Provider;
            List<AppModule> userModuleEntities;
            //权限为空时，根据当前登录的用户Id,获取权限模块数据
            if (op.ModulePermission == null)
            {
                op.ModulePermission = userModuleEntities = _appModuleService.GetUserModules(op.CurrentUser.UserId);
            }
            else
            {
                userModuleEntities = op.ModulePermission;
            }

            List<AppModule> userMenuEntities = userModuleEntities.Where(u => u.TypeCode != "Button" && u.TypeCode != "Permission").ToList();

            List<TreeNode> treeNodes = AppModule.ConvertTreeNodes(userMenuEntities);

            return treeNodes;
        }
    }
}
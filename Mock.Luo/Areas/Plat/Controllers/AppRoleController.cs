using Autofac;
using Mock.Code;
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
    public class AppRoleController : CrudController<AppRole, AppRoleViewModel>
    {
        // GET: Plat/AppRole


        private readonly IAppRoleRepository _service;
        private readonly IUserRoleRepository _urService;
        public AppRoleController(IAppRoleRepository service, IUserRoleRepository _urService, IComponentContext container) : base(container)
        {
            this._service = service;
            this._urService = _urService;
        }

        public ActionResult GetRoleJson()
        {
            return Result(_service.GetRoleJson());
        }

        public ActionResult GetDataGrid(string search = "")
        {
            return Result(_service.GetDataGrid(search));
        }
        /// <summary>
        /// 为角色分配用户
        /// </summary>
        /// <returns></returns>
        public ActionResult AllotUser()
        {
            return View();
        }

        public ActionResult GetAllotUserGrid(int roleId)
        {
            return Result(_urService.GetAllotUserGrid(roleId));
        }
        public ActionResult SaveMembers(string userIds, int roleId)
        {
            List<UserRole> urList = new List<UserRole>();
            if (!userIds.IsNullOrEmpty())
            {
                List<int> useridList = userIds.Split(',').Select(u => Convert.ToInt32(u)).ToList();
                foreach (var id in useridList)
                {
                    urList.Add(new UserRole
                    {
                        UserId = id,
                        RoleId = roleId
                    });
                }
            }
            return Result(_urService.SaveMembers(urList, roleId));
        }
    }
}
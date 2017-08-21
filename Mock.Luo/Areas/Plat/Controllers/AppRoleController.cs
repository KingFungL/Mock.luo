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
    public class AppRoleController : CrudController<AppRole,AppRoleViewModel>
    {
        // GET: Plat/AppRole


        IAppRoleRepository _service;
        public AppRoleController(IAppRoleRepository service, IComponentContext container) : base(container)
        {
            this._service = service;
        }

        public ActionResult GetRoleJson()
        {
            return Result(_service.GetRoleJson());
        }

        public ActionResult Form(int Id)
        {
            ViewBag.ViewModel = this.GetFormJson(Id);
            return View();
        }

        public ActionResult GetDataGrid(string search="")
        {
            return Result(_service.GetDataGrid(search));
        }

    }
}
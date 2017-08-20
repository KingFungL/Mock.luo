using Autofac;
using AutoMapper;
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
    public class AppUserController : CrudController<AppUser, AppUserViewModel>
    {
        // GET: Plat/AppUser
        IAppUserRepository _service;
        public AppUserController(IAppUserRepository service, IComponentContext container) : base(container)
        {
            this._service = service;
        }

        [HttpGet]
        public ActionResult Form(int id = 0)
        {

            ViewBag.ViewModel = this.GetFormJson(id);
            return View();

        }
        [HttpPost]
        public ActionResult GetGrid(Pagination pag, string LoginName = "", string Email = "")
        {
            return Content(_service.GetDataGrid(pag, LoginName, Email).ToJson());
        }
       
    }
}
using Mock.Code;
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
    public class AppUserController : BaseController
    {
        // GET: Plat/AppUser
        IAppUserRepository _service;
        public AppUserController(IAppUserRepository service)
        {
            this._service = service;
        }

        [HttpGet]
        public ActionResult Form(int id)
        {
            AppUser userEntity;
            if (id == 0)
            {
                userEntity = new AppUser { };
            }
            else
            {
                userEntity = _service.FindEntity(id);
            }
            return View(userEntity);

        }
        [HttpPost]
        public ActionResult GetGrid(Pagination pag)
        {
            return Content(_service.GetDataGrid(pag).ToJson());
        }
        [HttpPost]
        public ActionResult Edit(AppUser userEntity)
        {
            _service.Edit(userEntity);
            return Success();
        }
    }
}
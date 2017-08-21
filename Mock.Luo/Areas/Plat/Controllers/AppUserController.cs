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
            if (id == 0)
            {
                ViewBag.ViewModel = this.GetFormJson(id).ToJson();
            }
            else
            {
                var userEntity = _service.IQueryable(u => u.Id == id).Select(u => new {
                    u.Id,
                    u.LoginName,
                    u.NickName,
                    u.Email,
                    roleIds = u.UserRoles.Select(r => new {
                        id = r.RoleId,
                        text = r.AppRole.RoleName
                    }).ToList()
                }).FirstOrDefault();
                ViewBag.ViewModel = userEntity.ToJson();
            }
            return View();

        }
        [HttpPost]
        public ActionResult GetDataGrid(Pagination pag, string LoginName = "", string Email = "")
        {
            return Content(_service.GetDataGrid(pag, LoginName, Email).ToJson());
        }
       
        public ActionResult SubmitForm(AppUser userEntity,string roleIds)
        {
            AjaxResult result = _service.IsRepeat(userEntity);

            //用户名或邮箱重复
            if (result.state == ResultType.error.ToString())
            {
                return Content(result.ToJson());
            }

            _service.SubmitForm(userEntity, roleIds);

            return Success();

        }
    }
}
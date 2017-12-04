﻿using Autofac;
using AutoMapper;
using Mock.Code;
using Mock.Data.Models;
using Mock.Domain;
using Mock.Luo.Areas.Plat.Models;
using Mock.Luo.Controllers;
using Mock.Luo.Generic.Filters;
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
        [HandlerAuthorize]
        public override ActionResult Form(int id = 0)
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
                    u.StatusCode,
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
        [HandlerAuthorize]
        public ActionResult GetDataGrid(Pagination pag, string LoginName = "", string Email = "")
        {
            return Content(_service.GetDataGrid(pag, LoginName, Email).ToJson());
        }

        [HandlerAuthorize]
        public ActionResult SubmitForm(AppUser userEntity,string roleIds)
        {
            if (!ModelState.IsValid)
            {
                return Error(ModelState);
            }
            AjaxResult result = _service.IsRepeat(userEntity);

            //用户名或邮箱重复
            if (result.state == ResultType.error.ToString())
            {
                return Content(result.ToJson());
            }

            _service.SubmitForm(userEntity, roleIds);

            return Success();

        }
        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="Id">主键</param>
        /// <returns></returns>
        [HandlerAuthorize]
        public ActionResult ResetPassword(int Id)
        {
            _service.ResetPassword(new AppUser { Id = Id }, "1234");
            return Success();
        }
    }
}
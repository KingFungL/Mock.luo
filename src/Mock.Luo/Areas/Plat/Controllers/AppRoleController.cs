﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Autofac;
using Mock.Data.Models;
using Mock.Domain.Interface;
using Mock.Luo.Areas.Plat.Models;
using Mock.Luo.Controllers;
using Mock.Luo.Generic.Filters;

namespace Mock.Luo.Areas.Plat.Controllers
{
    public class AppRoleController : CrudController<AppRole, AppRoleViewModel>
    {
        // GET: Plat/AppRole


        private readonly IAppRoleRepository _appRoleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        public AppRoleController(IAppRoleRepository appRoleRepository, IUserRoleRepository userRoleRepository, IComponentContext container) : base(container)
        {
            this._appRoleRepository = appRoleRepository;
            this._userRoleRepository = userRoleRepository;
        }
        /// <summary>
        /// 角色下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRoleJson()
        {
            return Result(_appRoleRepository.GetRoleJson());
        }

        public ActionResult GetDataGrid(string search = "")
        {
            return Result(_appRoleRepository.GetDataGrid(search));
        }
        /// <summary>
        /// 为角色分配用户视图
        /// </summary>
        /// <returns></returns>
        [HandlerAuthorize]
        public ActionResult AllotUser()
        {
            return View();
        }
        /// <summary>
        /// 该角色下分配的用户数据
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public ActionResult GetAllotUserGrid(int roleId)
        {
            return Result(_userRoleRepository.GetAllotUserGrid(roleId));
        }

        /// <summary>
        /// 为角色分配用户
        /// </summary>
        /// <param name="userIds">以逗号分隔的用户Id字符串</param>
        /// <param name="roleId">单个角色Id</param>
        /// <returns></returns>
        [HandlerAuthorize]
        public ActionResult SaveMembers(string userIds, int roleId)
        {
            List<AppUserRole> urList = new List<AppUserRole>();
            if (!userIds.IsNullOrEmpty())
            {
                List<int> useridList = userIds.Split(',').Select(u => Convert.ToInt32(u)).ToList();
                foreach (var id in useridList)
                {
                    urList.Add(new AppUserRole
                    {
                        UserId = id,
                        RoleId = roleId
                    });
                }
            }
            return Result(_userRoleRepository.SaveMembers(urList, roleId));
        }
        /// <summary>
        /// 为角色分配权限视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize]
        public ActionResult AllotAuthorize()
        {
            return View();
        }
        /// <summary>
        /// 保存角色下的权限信息
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="data">权限树数据</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAuthorize]
        public ActionResult SaveAuthorize(int roleId,string data)
        {
            List<int> moduleIds = new List<int>();

            if (!string.IsNullOrEmpty(data))
            {
                moduleIds = data.Split(',').Select(u => Convert.ToInt32(u)).ToList();
            }

            List<AppRoleModule> roleModules = new List<AppRoleModule>();

            DateTime now = DateTime.Now;
            foreach (var moduleId in moduleIds)
            {
                AppRoleModule entity = new AppRoleModule
                {
                    RoleId = roleId,
                    ModuleId = moduleId
                };
                roleModules.Add(entity);
            }
            _appRoleRepository.SaveAuthorize(roleId, roleModules);
            return Success();
        }

    }
}
using System.Linq;
using System.Web.Mvc;
using Autofac;
using Mock.Code.Json;
using Mock.Code.Web;
using Mock.Data.Models;
using Mock.Domain.Interface;
using Mock.Luo.Areas.Plat.Models;
using Mock.Luo.Controllers;
using Mock.Luo.Generic.Filters;

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
                var userEntity = _service.Queryable(u => u.Id == id).Select(u => new {
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
        public ActionResult GetDataGrid(PageDto pag, string loginName = "", string email = "")
        {
            return Content(_service.GetDataGrid(pag, loginName, email).ToJson());
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
            if (result.State == ResultType.Error.ToString())
            {
                return Content(result.ToJson());
            }

            _service.SubmitForm(userEntity, roleIds);

            return Success();

        }
        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HandlerAuthorize]
        public ActionResult ResetPassword(int id)
        {
            _service.ResetPassword(new AppUser { Id = id }, "1234");
            return Success();
        }
    }
}
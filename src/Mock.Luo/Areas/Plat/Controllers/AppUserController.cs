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
        private readonly IAppUserRepository _appUserRepository;
        public AppUserController(IAppUserRepository appUserRepository, IComponentContext container) : base(container)
        {
            this._appUserRepository = appUserRepository;
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
                var userEntity = _appUserRepository.Queryable(u => u.Id == id).Select(u => new {
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
            return Content(_appUserRepository.GetDataGrid(pag, loginName, email).ToJson());
        }

        [HandlerAuthorize]
        public ActionResult SubmitForm(AppUser userEntity,string roleIds)
        {
            if (!ModelState.IsValid)
            {
                return Error(ModelState);
            }
            AjaxResult result = _appUserRepository.IsRepeat(userEntity);

            //用户名或邮箱重复
            if (result.State == ResultType.Error.ToString())
            {
                return Content(result.ToJson());
            }

            _appUserRepository.SubmitForm(userEntity, roleIds);

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
            _appUserRepository.ResetPassword(new AppUser { Id = id }, "1234");
            return Success();
        }
    }
}
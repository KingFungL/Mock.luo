using Mock.Code;
using Mock.Data;
using Mock.Domain;
using Mock.Luo.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mock.Luo.Controllers
{
    [Skip]
    public class LoginController : BaseController
    {
        // GET: Login

        private readonly IAppUserRepository _service;
        public LoginController(IAppUserRepository _service)
        {
            this._service = _service;
        }


        public ActionResult Mock()
        {
            OperatorProvider op = OperatorProvider.Provider;
            op.CurrentUser = new OperatorModel
            {
                UserId = 2,
                IsSystem = true,
                LoginName = "admin",
                LoginToken = Guid.NewGuid().ToString(),
                UserCode = "1234",
            };
            HttpRuntime.Cache[op.CurrentUser.UserId.ToString()] = Session.SessionID;
            return Success();
        }
        /// <summary>
        ///   登录接口验证
        /// </summary>
        /// <param name="loginName">登录名/邮箱</param>
        /// <param name="pwd">密码</param>
        /// <param name="code">验证码</param>
        /// <param name="token">唯一上下文token</param>
        /// <returns></returns>

        public ActionResult CheckLogin(string loginName, string pwd, bool rememberMe = false, string code = "", string token = "")
        {
            //if (code.IsNullOrEmpty())
            //{
            //    return Error("验证码不能为空！");
            //}
            if (loginName.IsNullOrEmpty())
            {
                return Error("用户名不能为空！");
            }
            if (pwd.IsNullOrEmpty())
            {
                return Error("密码不能为空！");
            }
            //当token不为null,且等于当前上下文的token时，直接验证登录
            if (!token.IsNullOrEmpty())
            {
                if (token == OperatorProvider.Provider.CurrentToken)
                {
                    return Result(_service.CheckLogin(loginName, pwd, rememberMe));
                }
            }

            //if (code.IsNotNullOrEmpty() && code.ToLower() != OperatorProvider.Provider.CurrentCode)
            //{
            //    return Error("验证码出错");
            //}

            return Result(_service.CheckLogin(loginName, pwd, rememberMe));

        }
    }
}
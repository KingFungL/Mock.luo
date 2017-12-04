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

        /// <summary>
        /// 默认页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Default()
        {
            return View();
        }

        public ActionResult GetAuthCode()
        {
            return File(new VerifyCode().GetVerifyCode(), @"image/Gif");
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


        public ActionResult PwdReset()
        {
            return View();
        }

        #region 重置密码，发送验证码 SmsCode
        public ActionResult SmsCode(string email, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Error("验证码不能为空！");
            }
            else if (string.IsNullOrEmpty(email))
            {
                return Error("邮箱不能为空！");
            }
            else if (op.CurrentCode != code)
            {
                return Error("验证码错误，请重新输入");
            }
            else
            {
             return Result(_service.SmsCode(email));
            }
        }
        #endregion

        #region 计时器60s过后，再次发送验证码
        public ActionResult SmsCodeAgain(string account)
        {
            if (string.IsNullOrEmpty(account))
            {
              return Error("参数出错!");
            }
            else
            {
              return Result(_service.SmsCode(account));
            }
        }
        #endregion

        #region 重置密码
        public ActionResult ResetPwd(string pwdtoken, string account, string newpwd, string emailcode)
        {
            if (string.IsNullOrEmpty(pwdtoken))
            {
                return Error("token不能为空!");
            }
            if (string.IsNullOrEmpty(account))
            {
                return Error("帐号不能为空!");
            }
            if (string.IsNullOrEmpty(newpwd))
            {
                return Error("新密码不能为空!");
            }
            if (string.IsNullOrEmpty(emailcode))
            {
                return Error("邮箱验证码不能为空!");
            }

            AjaxResult amm = _service.ResetPwd(pwdtoken, account, newpwd, emailcode);
            return Content(amm.ToJson());
        }
        #endregion
    }
}
using System;
using System.Web.Mvc;
using Mock.Code;
using Mock.Code.Extend;
using Mock.Data.ExtensionModel;
using Mock.luo.Models;

namespace Mock.luo.Controllers
{
    public class GenericController : BaseController
    {
        // GET: Generic
        public ActionResult EmailTemplate()
        {
            var viewModel = new EmailViewModel
            {
                ToUserName = "、天上有木月",
                Title = "有人给您留了言",
                Content = "呵呵呵呵呵呵呵。。",
                FromUserName = "如莫",
                Date = DateTime.Now.ToDateTimeString()
            };
            return View(viewModel);
        }

        public ActionResult GetUserInfo()
        {
            return View();
        }

        /// <summary>
        /// 激活邮件模板
        /// </summary>
        /// <returns></returns>

        public ActionResult ActiveEmailTemplate()
        {
            EmailViewModel model = new EmailViewModel();
            model.ToUserName = "710277267@qq.com";
            model.Link = "http://igeekfan.cn/App/ActiveEmail?token=g123029820984208429842042&a=323323323223";


            return View(model);
        }

        /// <summary>
        /// 发送验证码模板
        /// </summary>
        /// <returns></returns>
        public ActionResult SendCodeTemplate()
        {

            EmailViewModel model = new EmailViewModel();
            model.ToUserName = "710277267@qq.com";
            model.Code = "1234";

            return View(model);
        }
        /// <summary>
        /// 找回密码模板
        /// </summary>
        /// <returns></returns>
        public ActionResult PwdReSetTemplate()
        {
            ResetPwd resetpwdEntry = new ResetPwd
            {
                UserId = (int)1,
                ModifyPwdToken = Utils.GuId(),
                PwdCodeTme = DateTime.Now,
                ModfiyPwdCode = Utils.RndNum(6),
                LoginName = "admin",
                NickName="、天上有木月"
            };

            return View(resetpwdEntry);
        }
    }
}
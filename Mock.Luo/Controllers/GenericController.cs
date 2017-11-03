using Mock.Code;
using Mock.Luo.Models;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mock.Luo.Controllers
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

        public ActionResult ActiveEmailTemplate()
        {
            EmailViewModel model = new EmailViewModel();
            model.ToUserName = "710277267@qq.com";
            model.Link = "http://127.0.0.1/App/ActiveEmail?token=g123029820984208429842042&a=323323323223";


            return View(model);
        }

        public ActionResult SendCodeTemplate()
        {

            EmailViewModel model = new EmailViewModel();
            model.ToUserName = "710277267@qq.com";
            model.Code = "1234";

            return View(model);
        }

    }
}
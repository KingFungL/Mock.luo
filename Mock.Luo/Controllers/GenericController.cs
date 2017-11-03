using Mock.Code;
using Mock.Luo.Models;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
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



    }
}
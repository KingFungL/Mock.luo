using Mock.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mock.Luo.Controllers
{
    [Skip]
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HaveLogin()
        {
            return View();
        }

        public ActionResult ReturnToLogin()
        {
            return View();
        }

        public ActionResult E500()
        {
            return View();
        }

        public ActionResult E404()
        {
            return View();
        }
        public ActionResult LockScreen()
        {
            return View();
        }

    }
}
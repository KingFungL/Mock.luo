using Mock.Luo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mock.Luo.Areas.Mock.Controllers
{
    public class EditorController : BaseController
    {
        //
        // GET: /Mock/Editor/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MarkDownView()
        {
            return View();
        }

        public ActionResult CodeView()
        {
            return View();
        }
    }
}

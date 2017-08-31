
using Mock.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mock.Luo.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IRedisHelper iRedisHelper;
        public HomeController(IRedisHelper iRedisHelper)
        {
            this.iRedisHelper = iRedisHelper;
        }
        public ActionResult MainView()
        {
            return View();
        }

        public ActionResult DatalistView()
        {
            return View();
        }

        public ActionResult BlogView()
        {
            //iRedisHelper.StringSet<string>("key", "我是罗志强，这是一个testredis。net版");

            //string value = iRedisHelper.StringGet<string>("key");

            return View();
        }

        public ActionResult TestView()
        {
            return View();
        }

        public ActionResult GetTreeJson()
        {
            
            return View();
        }


    }
}

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

        public ActionResult UploadFileView()
        {
            return View();
        }

        public ActionResult BlogView()
        {
            //iRedisHelper.StringSet<string>("key", "我是罗志强，这是一个testredis。net版");

            //string value = iRedisHelper.StringGet<string>("key");

            return View();
        }

        public ActionResult GetTreeGrid()
        {
            var result = new List<object>();
            result.Add(new { Id = 1, Name = "百度科技", Desc = "搜索巨头" });
            result.Add(new { Id = 2, Name = "百度事业部", Desc = "搜索巨头", ParentId = 1 });
            result.Add(new { Id = 3, Name = "百度人事部", Desc = "搜索巨头", ParentId = 1 });
            result.Add(new { Id = 11, Name = "百度HH部", Desc = "搜索巨头", ParentId = 2 });
            result.Add(new { Id = 4, Name = "百度行政", Desc = "搜索巨头", ParentId = 1 });
            result.Add(new { Id = 5, Name = "百度YY部", Desc = "搜索巨头", ParentId = 1 });
            result.Add(new { Id = 12, Name = "百度BB部", Desc = "搜索巨头", ParentId = 2 });
            result.Add(new { Id = 6, Name = "搜狐科技", Desc = "IT" });
            result.Add(new { Id = 7, Name = "搜狐信息部", Desc = "IT", ParentId = 6 });
            result.Add(new { Id = 8, Name = "搜狐人事", Desc = "IT", ParentId = 6 });
            result.Add(new { Id = 9, Name = "搜狐事业部", Desc = "IT", ParentId = 6 });
            result.Add(new { Id = 10, Name = "搜狐事业子部", Desc = "IT", ParentId = 9 });
            return Content(JsonHelper.SerializeObject(result));
        }
    }
}
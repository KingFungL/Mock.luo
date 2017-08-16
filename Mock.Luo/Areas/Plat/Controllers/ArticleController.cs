using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mock.Domain;
using Mock.Code;
using Mock.Data.Models;
using Mock.Luo.Controllers;
using Mock.Data;

namespace Mock.Luo.Areas.Plat.Controllers
{
    public class ArticleController : BaseController
    {
        // GET: Plat/Article
        IArticleRepository _service;
        public ArticleController(IArticleRepository service)
        {
            this._service = service;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetGrid(Pagination pag)
        {
            return Content(_service.GetDataGrid(pag).ToJson());
        }

        public ActionResult Edit(Article entity)
        {

            return Success();
        }

        /// <summary>
        /// 最新文章
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRecentArticle()
        {
            return Result(_service.GetRecentArticle(5));
        }


        /// <summary>
        /// 得到博客列表页面
        /// </summary>
        /// <param name="pag"></param>
        /// <returns></returns>
        public ActionResult GetIndexGird(Pagination pag)
        {
            DataGrid dg = _service.GetIndexGird(pag);
            return Content(dg.ToJson());
        }
    }
}
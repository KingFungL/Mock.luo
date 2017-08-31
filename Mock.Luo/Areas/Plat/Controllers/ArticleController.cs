using Autofac;
using Mock.Code;
using Mock.Data;
using Mock.Data.Models;
using Mock.Domain;
using Mock.Luo.Areas.Plat.Models;
using Mock.Luo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Mock.Luo.Areas.Plat.Controllers
{
    public class ArticleController : CrudController<Article, ArticleViewModel>
    {
        // GET: Plat/Article
        IArticleRepository _service;
        public ArticleController(IArticleRepository service, IComponentContext container) : base(container)
        {
            this._service = service;
        }

        public ActionResult GetDataGrid(Pagination pag)
        {
            return Content(_service.GetDataGrid(pag).ToJson());
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
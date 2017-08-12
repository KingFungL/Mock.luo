using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mock.Domain;
using Mock.Code;

namespace Mock.Luo.Areas.Plat.Controllers
{
    public class ArticleController : Controller
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
    }
}
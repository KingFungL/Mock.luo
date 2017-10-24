using Mock.Code;
using Mock.Data.Dto;
using Mock.Data.Models;
using Mock.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mock.Luo.Controllers
{
    [Skip]
    public class AppController : BaseController
    {
        private readonly IItemsDetailRepository _itemsDetailRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IReviewRepository _reviewRepository;
        // GET: App
        public AppController(IItemsDetailRepository itemsDetailRepository,
            IArticleRepository articleRepository,
            IReviewRepository reviewRepository
            )
        {
            this._itemsDetailRepository = itemsDetailRepository;
            this._articleRepository = articleRepository;
            this._reviewRepository = reviewRepository;

        }

        public override ActionResult Index()
        {
            List<TreeSelectModel> tagItemsList = _itemsDetailRepository.GetCombobox("Tag");
            tagItemsList.RemoveAt(0);

            ViewData["Tag"] = tagItemsList;

            List<ArticleDto> LatestArticles = _articleRepository.GetRecentArticle(5);

            ViewData["LatestArticles"] = LatestArticles;

            List<Article> HotArticles = _articleRepository.GetHotArticle(8);

            ViewData["HotArticles"] = HotArticles;


            //吐槽

            List<ReplyDto> SpitslotList = _reviewRepository.GetRecentReview(8);
            ViewData["SpitslotList"] = SpitslotList;
            return base.Index();
        }

        public override ActionResult Detail(int Id)
        {
            string TypeName = Request["TypeName"];
            Article entry = _articleRepository.FindEntity(Id);
            entry.Content = Server.UrlDecode(entry.Content);
            ViewData["Article"] = entry;
            ViewBag.TypeName = TypeName;
            return base.View();
        }

    }
}
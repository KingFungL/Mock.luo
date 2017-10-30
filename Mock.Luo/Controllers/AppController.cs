using Mock.Code;
using Mock.Code.Helper;
using Mock.Data;
using Mock.Data.Dto;
using Mock.Data.Extensions;
using Mock.Data.Models;
using Mock.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly IRedisHelper _iredisHelper;
        // GET: App
        public AppController(IItemsDetailRepository itemsDetailRepository,
            IArticleRepository articleRepository,
            IReviewRepository reviewRepository,
            IRedisHelper iredisHelper
            )
        {
            this._itemsDetailRepository = itemsDetailRepository;
            this._articleRepository = articleRepository;
            this._reviewRepository = reviewRepository;
            this._iredisHelper = iredisHelper;

        }

        #region 博客首页
        public override ActionResult Index()
        {
            //标签
            List<TreeSelectModel> tagItemsList = _itemsDetailRepository.GetCombobox("Tag");
            tagItemsList.RemoveAt(0);
            ViewData["Tag"] = tagItemsList;

            //最新文章
            List<ArtDetailDto> LatestArticles = _articleRepository.GetRecentArticle(5);
            ViewData["LatestArticles"] = LatestArticles;

            //最热文章
            List<Article> HotArticles = _articleRepository.GetHotArticle(8);
            ViewData["HotArticles"] = HotArticles;

            //吐槽
            List<ReplyDto> SpitslotList = _reviewRepository.GetRecentReview(8);
            ViewData["SpitslotList"] = SpitslotList;

            //统计
            SiteStatistics site = _articleRepository.GetSiteData();

            ViewData["Site"] = site;

            //轻松时刻
            ViewData["JustFun"] = _iredisHelper.UnitOfWork(string.Format(ConstHelper.App, "JustFun"), () =>
              {
                  List<ArtDetailDto> justFunList = _articleRepository.GetArticleList(_articleRepository.IQueryable(u => u.DeleteMark == false && u.ItemsDetail.ItemCode == CategoryCode.justfun.ToString())).OrderByDescending(u => u.Id).Take(5).ToList();
                  if (justFunList.Count > 0)
                  {
                      justFunList[0].Content = Server.UrlDecode(justFunList[0].Content);
                  }
                  return justFunList;
              });

            //人生感悟
            ViewData["FellLife"] = _iredisHelper.UnitOfWork(string.Format(ConstHelper.App, "FellLife"), () =>
            {
                List<ArtDetailDto> feLifeList = _articleRepository.GetArticleList(_articleRepository.IQueryable(u => u.DeleteMark == false && u.ItemsDetail.ItemCode == CategoryCode.feelinglife.ToString())).OrderByDescending(u => u.Id).Take(5).ToList();
                if (feLifeList.Count > 0)
                {
                    feLifeList[0].Content = Server.UrlDecode(feLifeList[0].Content);
                }
                return feLifeList;
            });

            return base.Index();
        }
        #endregion

        #region 博客文章详情页
        public override ActionResult Detail(int Id)
        {
            IQueryable<Article> artiQuaryable = _articleRepository.IQueryable(u => u.Id == Id && u.DeleteMark == false);
            ArtDetailDto entry = _articleRepository.GetArticleList(artiQuaryable).FirstOrDefault();
            if (entry == null) throw new ArgumentNullException("根据Id,我去查了，但文章就是未找到！");
            //找到当前的上一个，下一个的文章

            Expression<Func<Article, bool>> nextlambda = u => u.Id > Id && u.DeleteMark == false;

            var next = _articleRepository.IQueryable(nextlambda).FirstOrDefault();
            if (next != null)
            {
                entry.NextPage = new BaseDto
                {
                    Id = next.Id,
                    text = next.Title
                };
            }
            Expression<Func<Article, bool>> previouslabmda = u => u.Id < Id && u.DeleteMark == false;
            var pre = _articleRepository.IQueryable(previouslabmda).OrderByDescending(u => u.Id).FirstOrDefault();
            if (pre != null)
            {
                entry.PreviousPage = new BaseDto
                {
                    Id = pre.Id,
                    text = pre.Title
                };
            }

            IQueryable<Article> queryable = _articleRepository.IQueryable(u => u.Id == Id);
            _articleRepository.Update(queryable, u => new Article
            {
                ViewHits = entry.ViewHits + 1
            });

            entry.Content = Server.UrlDecode(entry.Content);
            ViewData["ArticleDto"] = entry;

            ViewBag.AId = Id;

            return base.View();
        }
        #endregion

        /// <summary>
        /// 分类文章|标签
        /// </summary>
        /// <param name="category"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public ActionResult Category(string category = "", string tag = "", string archive = "")
        {
            if (category.IsNullOrEmpty() && tag.IsNullOrEmpty() && archive.IsNullOrEmpty())
            {
                throw new ArgumentNullException("参数不正确！！!");
            }
            Pagination pag = new Pagination
            {
                sort = "Id",
                order = "desc",
                limit = 10,
                offset = 0
            };
            DataGrid dg = _articleRepository.GetCategoryTagGrid(pag, category, tag, archive);
            string TypeName = "";
            if (category.IsNotNullOrEmpty() || tag.IsNotNullOrEmpty())
            {

                TypeName = _itemsDetailRepository.IQueryable(r => r.ItemCode == category || r.ItemCode == tag).Select(r => r.ItemName).FirstOrDefault();
            }
            else
            {
                TypeName = archive;
            }
            ViewBag.TypeName = TypeName.IsNullOrEmpty() ? "亲，您迷路了啊！|、天上有木月" : TypeName;

            ViewBag.ViewModel = dg.ToJson();

            return View();
        }

        public ActionResult LeaveMsg()
        {
            return View();
        }

        #region 获取文章相关内容：文章归档，置顶文章，分类，标签，相关文章，随机文章
        public ActionResult GetRelateList(int Id)
        {
            var ardList = _articleRepository.GetRelateDtoByAId(Id);

            return Result(ardList);
        }
        #endregion

        public ActionResult NotFound()
        {
            return View();
        }
    }
}
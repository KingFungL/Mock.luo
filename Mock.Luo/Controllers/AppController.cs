using Mock.Code;
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
            SiteStatistics site;
            OperatorProvider op = OperatorProvider.Provider;
            var artQuery = _articleRepository.IQueryable(u => u.DeleteMark == false);

            if (op.Session["site"] == null)
            {
                site = new SiteStatistics
                {
                    ArticleCount = artQuery.Count(),
                    PointViewCount = artQuery.Sum(u => u.PointQuantity),
                    ArticleTypeCount = _itemsDetailRepository.IQueryable(u => u.DeleteMark == false && u.Items.EnCode == EnCode.FTypeCode.ToString()).Count(),
                    TagCount = _itemsDetailRepository.IQueryable(u => u.DeleteMark == false && u.Items.EnCode == EnCode.Tag.ToString()).Count(),
                    ReplyCount = _reviewRepository.IQueryable(u => u.DeleteMark == false).Count(),
                    ViewHitCount = artQuery.Sum(r => r.ViewHits)
                };
                op.Session["site"] = site;
            }
            else
            {
                site = op.Session["site"] as SiteStatistics;
            }

            ViewData["Site"] = site;
            //轻松时刻
            List<ArtDetailDto> justFunList = _articleRepository.GetArticleList(_articleRepository.IQueryable(u => u.DeleteMark == false && u.ItemsDetail.ItemCode == "justfun")).OrderByDescending(u => u.Id).Take(5).ToList();
            if (justFunList.Count > 0)
            {
                justFunList[0].Content = Server.UrlDecode(justFunList[0].Content);
            }
            ViewData["JustFun"] = justFunList;

            //人生感悟
            List<ArtDetailDto> feLifeList = _articleRepository.GetArticleList(_articleRepository.IQueryable(u => u.DeleteMark == false && u.ItemsDetail.ItemCode == "feelinglife")).OrderByDescending(u => u.Id).Take(5).ToList();
            if (feLifeList.Count > 0)
            {
                feLifeList[0].Content = Server.UrlDecode(feLifeList[0].Content);
            }
            ViewData["FellLife"] = feLifeList;


            return base.Index();
        }

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

        public ActionResult Category(string category)
        {
            return View();
        }

        public ActionResult LeaveMsg()
        {
            return View();
        }

        public ActionResult GetRelateList(int Id)
        {

            //文章归档
            var archiveList = _articleRepository.IQueryable(u => u.DeleteMark == false && u.IsAudit == true).GroupBy(u => u.Archive).Select(u => new
            {
                u.Key,
                u.FirstOrDefault().Archive,
                count = u.Count(),
            }).ToList();

            IQueryable<Article> iQ = _articleRepository.IQueryable(u => u.DeleteMark == false);
            //文章归档
            List<BaseDto> archiveFile = archiveList.Select(u => new BaseDto
            {
                text = u.Archive,
                code = u.count.ToString()
            }).ToList();

            //从文章列表中取出5条博主最后新增时间的置顶文章
            List<BaseDto> recommendArt = _articleRepository.IQueryable(u => u.DeleteMark == false && u.IsAudit == true).OrderByDescending(u => u.CreatorTime).Take(5).ToList().Select(u => new BaseDto { Id = u.Id, text = u.Title }).ToList();

            //取出分类目录:分类编码，分类名称
            List<BaseDto> category = _itemsDetailRepository.IQueryable(u => u.Items.EnCode == EnCode.FTypeCode.ToString()).Select(u => new BaseDto { Id = u.Id, text = u.ItemName, code = u.Items.EnCode }).ToList();

            //取出分类FId,和文章对应的标签多个Id
            var iTagFid = iQ.Where(r => r.Id == Id).Select(r => new {
                r.FId,
                TagArts = r.TagArts.Select(u => new { u.AId,u.TagId,u.ItemsDetail.ItemCode,u.ItemsDetail.ItemName})
            }).FirstOrDefault();

            int? FId = iTagFid.FId;
            List<int> tagIdList = iTagFid.TagArts.Select(u => u.TagId).ToList();
            Expression<Func<Article, bool>> predicate = u => u.FId == FId;

            //文章对应的多个标签
            List<BaseDto> ArtTag = new List<BaseDto>();

            if (tagIdList.Count > 0)
            {
                predicate = predicate.Or(u => u.TagArts.Select(r => tagIdList.Contains(r.Id)).Count() > 0);
                ArtTag = iTagFid.TagArts.Select(u => new BaseDto {
                    Id = u.TagId,
                    text = u.ItemName,
                    code = u.ItemCode
                }).ToList();
            }

            //有关本文章的相关文章5条
            List<BaseDto> relateArt = iQ.Where(predicate)
            .OrderByNewId().Take(5).ToList().Select(u => new BaseDto { Id = u.Id, text = u.Title }).ToList();

            //随机文章
            List<BaseDto> randomArt = iQ.Select(u => new BaseDto { Id = u.Id, text = u.Title }).Take(5).ToList();

            ArtRelateDto ardList = new ArtRelateDto
            {
                RelateArt = relateArt,
                Category = category,
                RecommendArt = recommendArt,
                ArchiveFile = archiveFile,
                RandomArt = randomArt,
                ArtTag = ArtTag
            };

            return Result(ardList);
        }
    }
}
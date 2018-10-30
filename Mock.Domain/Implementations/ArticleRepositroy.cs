using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mock.Code.Extend;
using Mock.Code.Helper;
using Mock.Code.Web;
using Mock.Data.AppModel;
using Mock.Data.Dto;
using Mock.Data.Extensions;
using Mock.Data.Models;
using Mock.Data.Repository;
using Mock.Domain.Interface;

namespace Mock.Domain.Implementations
{
    /// <summary>
    /// 仓储实现层 ArticleRepositroy
    /// </summary>]
    public class ArticleRepositroy : RepositoryBase<Article>, IArticleRepository
    {
        #region Constructor

        private readonly IRedisHelper _iRedisHelper;
        public ArticleRepositroy(IRedisHelper iRedisHelper)
        {
            _iRedisHelper = iRedisHelper;
        }

        #endregion

        #region private methods
        private DataGrid GetGridByExpression(Expression<Func<Article, bool>> predicate, PageDto pag)
        {
            var dglist = this.Queryable(predicate).Where(pag).Select(t => new
            {
                t.Id,
                t.ItemsDetail.ItemCode,
                t.ItemsDetail.ItemName,
                t.CreatorTime,
                t.Title,
                t.Excerpt,
                //t.Content,
                t.ViewHits,
                t.CommentQuantity,
                t.Keywords,
                t.Source,
                thumbnail = t.Thumbnail,
                t.AppUser.LoginName,
                t.IsAudit,
                t.IsStickie,
                t.PointQuantity,
                t.Recommend
            }).ToList();

            return new DataGrid { Rows = dglist, Total = pag.Total };
        }
        #endregion

        public ArtDetailDto GetOneArticle(int id)
        {
            ArtDetailDto artEntry = Queryable(u => u.Id == id && u.DeleteMark == false).Select(r => new ArtDetailDto
            {
                TypeName = r.ItemsDetail.ItemName,
                TypeCode = r.ItemsDetail.ItemCode,
                NickName = r.AppUser.NickName,
                Avatar = r.AppUser.Avatar,
                PersonSignature = r.AppUser.PersonSignature,
                Id = r.Id,
                Content = r.Content,
                Title = r.Title,
                CommentQuantity = r.CommentQuantity,
                Excerpt = r.Excerpt,
                CreatorUserId = r.CreatorUserId,
                CreatorTime = r.CreatorTime,
                ViewHits = r.ViewHits,
                Thumbnail = r.Thumbnail,
                PointQuantity = r.PointQuantity
            }).FirstOrDefault();

            artEntry.TimeSpan = TimeHelper.GetDateFromNow(artEntry.CreatorTime.ToDateTime());

            return artEntry;
        }

        #region 抽象取文章 列表数据
        public List<ArtDetailDto> GetArticleList(IQueryable<Article> artiQuaryable)
        {
            List<ArtDetailDto> artList = artiQuaryable.Select(r => new ArtDetailDto
            {
                TypeName = r.ItemsDetail.ItemName,
                TypeCode = r.ItemsDetail.ItemCode,
                NickName = r.AppUser.NickName,
                Avatar = r.AppUser.Avatar,
                PersonSignature = r.AppUser.PersonSignature,
                Id = r.Id,
                Title = r.Title,
                CommentQuantity = r.CommentQuantity,
                Excerpt = r.Excerpt,
                CreatorUserId = r.CreatorUserId,
                CreatorTime = r.CreatorTime,
                ViewHits = r.ViewHits,
                Thumbnail = r.Thumbnail,
            }).ToList();

            artList.ForEach(u =>
            {
                u.TimeSpan = TimeHelper.GetDateFromNow(u.CreatorTime.ToDateTime());
            });
            return artList;
        }
        #endregion

        #region 后台管理的分页列表数据
        public DataGrid GetDataGrid(PageDto pag, string search)
        {
            Expression<Func<Article, bool>> predicate = u => u.DeleteMark == false
                && (search == "" || u.Title.Contains(search) || u.AppUser.LoginName.Contains(search));

            return this.GetGridByExpression(predicate, pag);

        }
        #endregion

        /// <summary>
        /// 最新的文章| 缓存
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ArtDetailDto> GetRecentArticle(int count)
        {
            return _iRedisHelper.UnitOfWork(string.Format(ConstHelper.Article, "GetRecentArticle"), () =>
            {
                IQueryable<Article> artiQuaryable = this.Queryable(u => u.DeleteMark == false).OrderByDescending(r => r.Id).Take(count);
                return this.GetArticleList(artiQuaryable);
            });
        }


        #region 根据评论量，点赞量，阅读次数得到最火文章 | 缓存
        public List<Article> GetHotArticle(int count)
        {
            return _iRedisHelper.UnitOfWork(string.Format(ConstHelper.Article, "GetHotArticle"), () =>
            {
                return this.Queryable(u => u.DeleteMark == false).Select(u => new
                {
                    u.Id,
                    u.Title,
                    u.ViewHits,
                    u.CommentQuantity,
                    u.PointQuantity,
                    HotQuantity = u.ViewHits + u.CommentQuantity + u.PointQuantity
                }).OrderByDescending(r => r.HotQuantity).Take(count).ToList().Select(u => new Article
                {
                    Id = u.Id,
                    Title = u.Title,
                }).ToList();
            });
        }
        #endregion

        /// <summary>
        /// 得到分类|标签 文章的分页数据
        /// </summary>
        /// <param name="pag"></param>
        /// <param name="category"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public DataGrid GetCategoryTagGrid(PageDto pag, string category, string tag, string archive)
        {

            Expression<Func<Article, bool>> expression = u => u.DeleteMark == false;

            if (tag.IsNotNullOrEmpty())//tag:标签时，按照标签内容去查询列表数据
            {
                expression = expression.And(u => Enumerable.Where<TagArt>(u.TagArts, r => r.ItemsDetail.ItemCode == tag).Count() > 0);
            }
            else if (category.IsNotNullOrEmpty())
            {
                expression = expression.And(u => category == "" || u.ItemsDetail.ItemCode == category);
            }
            else
            {
                expression = expression.And(u => u.Archive == archive);
            }

            IQueryable<Article> iQueryableArt = base.Queryable(expression);
            //前台文章列表的特殊性，需要按照特定要求取出数据
            List<ArtDetailDto> rows = this.GetArticleList(iQueryableArt.Where(pag));

            return new DataGrid { Total = iQueryableArt.Count(), Rows = rows };

        }

        public SiteStatistics GetSiteData()
        {
            return _iRedisHelper.UnitOfWork(string.Format(ConstHelper.Article, "GetSiteData"), () =>
            {
                var artQuery = base.Queryable(u => u.DeleteMark == false);
                return new SiteStatistics
                {
                    ArticleCount = artQuery.Count(),
                    PointViewCount = artQuery.Sum(u => u.PointQuantity),
                    ArticleTypeCount = base.Db.Set<ItemsDetail>().AsNoTracking().Where(u => u.DeleteMark == false && u.Items.EnCode == EnCode.FTypeCode.ToString()).Count(),
                    TagCount = base.Db.Set<ItemsDetail>().AsNoTracking().Where(u => u.DeleteMark == false && u.Items.EnCode == EnCode.Tag.ToString()).Count(),
                    ReplyCount = base.Db.Set<Review>().AsNoTracking().Where(u => u.DeleteMark == false).Count(),
                    ViewHitCount = artQuery.Sum(r => r.ViewHits)
                };
            });

        }

        #region 根据文章Id得到相关内容
        public ArtRelateDto GetRelateDtoByAId(int id)
        {

            IQueryable<Article> iQ = base.Queryable(u => u.DeleteMark == false);

            //文章归档 | 可缓存
            List<BaseDto> archiveFile = _iRedisHelper.UnitOfWork(string.Format(ConstHelper.Article, "archiveFile"), () =>
            {
                return base.Queryable(u => u.DeleteMark == false && u.IsAudit == true).GroupBy(u => u.Archive).Select(u => new
                {
                    u.Key,
                    u.FirstOrDefault().Archive,
                    u.FirstOrDefault().CreatorTime,
                    count = u.Count(),
                }).OrderBy(u=>u.CreatorTime).ToList().Select(u => new BaseDto
                {
                    Text = u.Archive,
                    Code = u.count.ToString()
                }).ToList();

            });


            //从文章列表中取出5条博主最后新增时间的置顶文章 | 可缓存
            List<BaseDto> recommendArt = _iRedisHelper.UnitOfWork(string.Format(ConstHelper.Article, "recommendArt"), () =>
            {
                return base.Queryable(u => u.DeleteMark == false && u.IsAudit == true).OrderByDescending(u => u.CreatorTime).Take(5).ToList().Select(u => new BaseDto { Id = u.Id, Text = u.Title }).ToList();
            });

            //取出分类目录:分类编码，分类名称
            List<BaseDto> category = _iRedisHelper.UnitOfWork(string.Format(ConstHelper.Article, "category"), () =>
            {
                return base.Db.Set<ItemsDetail>().AsNoTracking().Where(u => u.Items.EnCode == EnCode.FTypeCode.ToString()).Select(u => new BaseDto { Id = u.Id, Text = u.ItemName, Code = u.ItemCode }).ToList();
            });

            //取出分类FId,和文章对应的标签多个Id
            var iTagFid = iQ.Where(r => r.Id == id).Select(r => new
            {
                r.FId,
                TagArts = r.TagArts.Select(u => new { u.AId, u.TagId, u.ItemsDetail.ItemCode, u.ItemsDetail.ItemName })
            }).FirstOrDefault();

            int? fId = iTagFid.FId;
            List<int> tagIdList = iTagFid.TagArts.Select(u => u.TagId).ToList();
            Expression<Func<Article, bool>> predicate = u => u.FId == fId;

            //文章对应的多个标签
            List<BaseDto> artTag = new List<BaseDto>();

            if (tagIdList.Count > 0)
            {
                predicate = predicate.Or(u => u.TagArts.Select(r => tagIdList.Contains(r.Id)).Count() > 0);
                artTag = iTagFid.TagArts.Select(u => new BaseDto
                {
                    Id = u.TagId,
                    Text = u.ItemName,
                    Code = u.ItemCode
                }).ToList();
            }

            //有关本文章的相关文章5条
            List<BaseDto> relateArt = iQ.Where(predicate)
            .OrderByNewId().Take(5).Select(u => new BaseDto { Id = u.Id, Text = u.Title }).ToList();

            //随机文章
            List<BaseDto> randomArt = iQ.Select(u => new BaseDto { Id = u.Id, Text = u.Title }).Take(5).ToList();

            ArtRelateDto ardList = new ArtRelateDto
            {
                RelateArt = relateArt,
                Category = category,
                RecommendArt = recommendArt,
                ArchiveFile = archiveFile,
                RandomArt = randomArt,
                ArtTag = artTag
            };
            return ardList;
        } 
        #endregion
    }
}

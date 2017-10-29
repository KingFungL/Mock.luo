using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock.Code;
using System.Linq.Expressions;
using Mock.Data.Dto;
using Mock.Code.Helper;

namespace Mock.Domain
{
    /// <summary>
    /// 仓储实现层 ArticleRepositroy
    /// </summary>]
    public class ArticleRepositroy : RepositoryBase<Article>, IArticleRepository
    {
        #region 后台管理的分页列表数据
        public DataGrid GetDataGrid(Pagination pag, string search)
        {
            Expression<Func<Article, bool>> predicate = u => u.DeleteMark == false
                && (search == "" || u.Title.Contains(search) || u.AppUser.LoginName.Contains(search));

            var dglist = this.IQueryable(predicate).Where(pag).Select(t => new
            {
                t.Id,
                t.ItemsDetail.ItemName,
                t.CreatorTime,
                t.Title,
                t.Excerpt,
                t.Content,
                t.ViewHits,
                t.CommentQuantity,
                t.Keywords,
                t.Source,
                t.thumbnail,
                t.AppUser.LoginName,
                t.IsAudit,
                t.IsStickie,
                t.PointQuantity,
                t.Recommend
            }).ToList();

            return new DataGrid { rows = dglist, total = pag.total };

        }
        #endregion

        /// <summary>
        /// 最新的文章
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ArtDetailDto> GetRecentArticle(int count)
        {
            IQueryable<Article> artiQuaryable = this.IQueryable(u => u.DeleteMark == false).OrderByDescending(r => r.Id).Take(count);
            return GetArticleList(artiQuaryable);
        }

        public List<ArtDetailDto> GetArticleList(IQueryable<Article> artiQuaryable)
        {
            return artiQuaryable.Select(r => new
            {
                u = r,
                TypeName = r.ItemsDetail == null ? "" : r.ItemsDetail.ItemName,
                TypeCode=r.ItemsDetail==null?"":r.ItemsDetail.ItemCode,
                NickName = r.AppUser.NickName,
                HeadHref = r.AppUser.HeadHref,
                PersonSignature= r.AppUser.PersonSignature
            }).ToList().Select(r => new ArtDetailDto
            {
                Id = r.u.Id,
                TypeName = r.TypeName,
                TypeCode=r.TypeCode,
                NickName = r.NickName,
                TimeSpan = TimeHelper.GetDateFromNow(r.u.CreatorTime.ToDateTime()),
                Title = r.u.Title,
                Content =  r.u.Content,
                CommentQuantity = r.u.CommentQuantity,
                Excerpt = r.u.Excerpt,
                CreatorUserId = r.u.CreatorUserId,
                CreatorTime = r.u.CreatorTime,
                ViewHits = r.u.ViewHits,
                thumbnail = r.u.thumbnail,
                HeadHref = r.HeadHref,
                PersonSignature=r.PersonSignature
            }).ToList();
        }

        public DataGrid GetIndexGird(Pagination pag)
        {
            var rows = this.IQueryable().OrderByDescending(r => r.Id).Where(pag).Select(r => new
            {
                r.Title,
                r.AppUser.LoginName,
                r.CreatorTime,
                r.Id,
                r.ItemsDetail.ItemName,
                ReviewCount = r.Reviews.Count(u => u.AId == r.Id),
                r.Excerpt,
                r.thumbnail
            }).ToList();
            return new DataGrid { rows = rows, total = pag.total };
        }

        /// <summary>
        /// 根据评论量，点赞量，阅读次数得到最火文章
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Article> GetHotArticle(int count)
        {
            return this.IQueryable(u => u.DeleteMark == false).Select(u => new
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
        }
        
    }
}

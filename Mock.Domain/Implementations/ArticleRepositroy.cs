using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock.Code;

namespace Mock.Domain
{
    /// <summary>
    /// 仓储实现层 ArticleRepositroy
    /// </summary>]
    public class ArticleRepositroy : RepositoryBase<Article>, IArticleRepository
    {
        #region 后台管理的分页列表数据
        public DataGrid GetDataGrid(Pagination pag)
        {

            var dglist = this.IQueryable(u => u.DeleteMark == false).Where(pag).Select(t => new
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
        public dynamic GetRecentArticle(int count)
        {
            return this.IQueryable(u => u.DeleteMark == false).OrderByDescending(r => r.Id).Select(r => new
            {
                r.Id,
                r.Title
            }).Take(count).ToList();
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
    }
}

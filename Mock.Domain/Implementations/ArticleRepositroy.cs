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
        public DataGrid GetDataGrid(Pagination pag)
        {

            var dglist = this.IQueryable(u=>u.DeleteMark==false).Where(pag).Select(t => new
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

        public dynamic GetFormById(int Id)
        {
            var d = this.IQueryable(u => u.Id == Id).Select(u => new
            {
            }).FirstOrDefault();
            return d;
        }

        public void Edit(Article entity)
        {

            if (entity.Id == 0)
            {
                entity.Create();
                entity.ViewHits = 0;
                entity.CommentQuantity = 0;
                entity.PointQuantity = 0;
                this.Insert(entity);
            }
            else
            {
                entity.Modify(entity.Id);
                string[] modifystr = { "FTypeCode", "Excerpt", "Title", "Keywords", "Source", "Excerpt", "Content", "thumbnail", "IsAudit", "Recommend", "IsStickie" };
                this.Update(entity, modifystr);
            }

        }

        public void DeleteForm(int keyValue)
        {
            Article entity = new Article { Id = keyValue };
            entity.Remove();
            this.Update(entity, "IsVisible");
        }

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

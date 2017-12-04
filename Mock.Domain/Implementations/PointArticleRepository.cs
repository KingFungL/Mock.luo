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
    public class PointArticleRepository : RepositoryBase<PointArticle>, IPointArticleRepository
    {
        /// <summary>
        /// 根据文章id得到点赞人信息 
        /// </summary>
        /// <param name="ArticeId">文章主键</param>
        /// <returns>DataGrid实体</returns>
        public DataGrid GetDataGrid(int ArticleId)
        {
            var rows = base.IQueryable(u => u.AId == ArticleId).Select(u => new
            {
                u.Id,
                u.Article.Title,
                u.AppUser.Email,
                u.AId,
                u.AppUser.LoginName,
                u.AddTime
            }).ToList();

            return new DataGrid { rows = rows, total = rows.Count() };
        }
    }
}

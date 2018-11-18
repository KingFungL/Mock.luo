using System.Linq;
using Mock.Data.AppModel;
using Mock.Data.Models;
using Mock.Data.Repository;
using Mock.Domain.Interface;

namespace Mock.Domain.Implementations
{
    public class PointArticleRepository : RepositoryBase<PointArticle>, IPointArticleRepository
    {
        /// <summary>
        /// 根据文章id得到点赞人信息 
        /// </summary>
        /// <param name="articleId">文章主键</param>
        /// <returns>DataGrid实体</returns>
        public DataGrid GetDataGrid(int articleId)
        {
            var rows = base.Queryable(u => u.AId == articleId).Select(u => new
            {
                u.Id,
                u.Article.Title,
                u.AId,
                u.AddTime,
                u.IP,
                u.Browser,
                u.Email,
                u.System,
                u.LoginName
            }).ToList();

            return new DataGrid { Rows = rows, Total = rows.Count() };
        }
    }
}

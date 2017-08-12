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

            var dglist = this.IQueryable().Where(pag).Select(u => new
            {
                u.Title,
                u.Content,
                u.CreatorUserId,
                u.AppUser.LoginName
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


    }
}

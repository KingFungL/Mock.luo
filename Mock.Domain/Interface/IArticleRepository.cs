using Mock.Code;
using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Domain
{
    public interface IArticleRepository : IRepositoryBase<Article>
    {
        /// <summary>
        /// 后台管理的分页列表数据
        /// </summary>
        /// <param name="pag"></param>
        /// <returns></returns>
        DataGrid GetDataGrid(Pagination pag);
        /// <summary>
        /// 得到最新的count篇文章
        /// </summary>
        /// <param name="count">条数</param>
        /// <returns></returns>
        dynamic GetRecentArticle(int count);
        /// <summary>
        /// 得到博客列表页面
        /// </summary>
        /// <param name="pag">分页条件</param>
        /// <returns></returns>
        DataGrid GetIndexGird(Pagination pag);
    }
}

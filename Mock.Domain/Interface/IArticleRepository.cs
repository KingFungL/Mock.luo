using Mock.Code;
using Mock.Data;
using Mock.Data.Dto;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Domain
{
    public interface IArticleRepository : IRepositoryBase<Article>
    {
        /// <summary>
        /// 得到单个文章内容
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ArtDetailDto GetOneArticle(int Id);

        /// <summary>
        /// 抽象取文章的列表数据 
        /// </summary>
        /// <param name="artiQuaryable"></param>
        /// <returns></returns>
        List<ArtDetailDto> GetArticleList(IQueryable<Article> artiQuaryable);
        /// <summary>
        /// 后台管理的分页列表数据
        /// </summary>
        /// <param name="pag"></param>
        /// <returns></returns>
        DataGrid GetDataGrid(Pagination pag,string search);
        /// <summary>
        /// 得到最新的count篇文章 | 缓存
        /// </summary>
        /// <param name="count">条数</param>
        /// <returns></returns>
        List<ArtDetailDto> GetRecentArticle(int count);

        /// <summary>
        /// 根据评论量，点赞量，阅读次数得到最火文章 | 缓存
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Article> GetHotArticle(int count);

        /// <summary>
        /// 得到分类|标签 文章的分页数据
        /// </summary>
        /// <param name="pag"></param>
        /// <param name="category"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        DataGrid GetCategoryTagGrid(Pagination pag, string category, string tag,string archive);

        /// <summary>
        /// 缓存 得到站点统计数据
        /// </summary>
        /// <returns></returns>
        SiteStatistics GetSiteData();

        /// <summary>
        /// 根据文章Id得到相关内容
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ArtRelateDto GetRelateDtoByAId(int Id);
    }
}

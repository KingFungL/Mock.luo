using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Mock.Code.Web;
using Mock.Data.AppModel;
using Mock.Data.Dto;
using Mock.Data.Models;
using Mock.Data.Repository;

namespace Mock.Domain.Interface
{
    public interface IReviewRepository : IRepositoryBase<Review>
    {
        /// <summary>
        /// 得到最新的count条回复信息 | 缓存
        /// </summary>
        /// <returns>最新回复消息</returns>
        List<ReplyDto> GetRecentReview(int count);
        /// <summary>
        /// 根据条件得到文章的评论分页数据，默认是全部,可根据文章id查看评论内容 
        /// </summary>
        /// <param name="pag">分页条件</param>
        /// <param name="email">邮箱</param>
        /// <param name="aId">文章主键</param>
        /// <returns>DataGrid实体</returns>
        DataGrid GetDataGrid(Expression<Func<Review, bool>> predicate, PageDto pag, string email, int aId,int flag);


    }
}

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
    /// 仓储实现层 ReviewRepository
    /// </summary>]
    public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
    {
        #region Constructor

        private readonly IRedisHelper _iRedisHelper;
        public ReviewRepository(IRedisHelper iRedisHelper)
        {
            _iRedisHelper = iRedisHelper;
        }
        #endregion

        #region 根据条件得到文章的评论分页数据，默认是全部,可根据文章id查看评论内容 
        /// <summary>
        /// 根据条件得到文章的评论分页数据,可根据文章id查看评论内容 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pag"></param>
        /// <param name="email"></param>
        /// <param name="aId"></param>
        /// <returns></returns>
        public DataGrid GetDataGrid(Expression<Func<Review, bool>> predicate, PageDto pag, string email, int aId)
        {
            predicate = predicate.And(u => u.DeleteMark == false
               && (email == "" || u.AuEmail.Contains(email))
               && (aId == 0 || u.AId == aId));

            var reviewList = base.Queryable(u => u.AId == aId).Select(u => new
            {
                u.PId,
                u.Id,
                u.AuName
            }).ToList();

            var dglist = base.Queryable(predicate).Where(pag).Select(u=>new {
                u.Article.Title,
                Avatar = u.AppUser == null ? u.Avatar : u.AppUser.Avatar,
                u=u
            }).ToList().Select(r => new
            {
                r.Title,
                r.Avatar,
                r.u.Id,
                r.u.AId,
                PName = reviewList.Where(s => s.Id == r.u.PId).Select(s => s.AuName).FirstOrDefault(),
                r.u.PId,
                r.u.Text,
                r.u.Ip,
                r.u.Agent,
                r.u.System,
                r.u.GeoPosition,
                r.u.UserHost,
                r.u.AuName,
                r.u.AuEmail,
                r.u.IsAduit,
                r.u.CreatorTime
            }).ToList();
            return new DataGrid { Rows = dglist, Total = pag.Total };
        }
        #endregion

        #region 得到最新的count条回复信息
        public List<ReplyDto> GetRecentReview(int count)
        {
            return _iRedisHelper.UnitOfWork(string.Format(ConstHelper.Review, "GetRecentReview"), () =>
            {
                return this.Queryable().OrderByDescending(u => u.Id).Take(count).Select(u => new
                {
                    u.Article.Title,
                    Avatar = u.AppUser != null ? u.AppUser.Avatar : u.Avatar,
                    u
                }).ToList().Select(r => new ReplyDto
                {
                    Id = r.u.Id,
                    AId = r.u.AId,
                    ArticleTitle = r.Title,
                    Text = r.u.Text,
                    NickName = r.u.AuName,
                    Avatar = r.Avatar
                }).ToList();
            });
        }
        #endregion
    }
}

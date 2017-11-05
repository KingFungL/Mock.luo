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
        /// <param name="Email"></param>
        /// <param name="AId"></param>
        /// <returns></returns>
        public DataGrid GetDataGrid(Expression<Func<Review, bool>> predicate, Pagination pag, string Email, int AId)
        {
            predicate = predicate.And(u => u.DeleteMark == false
               && (Email == "" || u.AuEmail.Contains(Email))
               && (AId == 0 || u.AId == AId));

            var reviewList = base.IQueryable(u => u.AId == AId).Select(u => new
            {
                u.PId,
                u.Id,
                u.AuName
            }).ToList();

            var dglist = base.IQueryable(predicate).Where(pag).Select(u=>new {
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
            return new DataGrid { rows = dglist, total = pag.total };
        }
        #endregion

        #region 得到最新的count条回复信息
        public List<ReplyDto> GetRecentReview(int count)
        {
            return _iRedisHelper.UnitOfWork(string.Format(ConstHelper.Review, "GetRecentReview"), () =>
            {
                return this.IQueryable().OrderByDescending(u => u.Id).Take(count).Select(u => new
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

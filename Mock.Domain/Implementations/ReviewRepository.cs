﻿using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock.Code;
using System.Linq.Expressions;
using Mock.Data.Dto;

namespace Mock.Domain
{
    /// <summary>
    /// 仓储实现层 ReviewRepository
    /// </summary>]
    public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
    {
        #region 根据条件得到文章的评论分页数据，默认是全部,可根据文章id查看评论内容 
        public DataGrid GetDataGrid(Pagination pag, string Email, int AId)
        {
            Expression<Func<Review, bool>> predicate = u => u.DeleteMark == false
            && (Email == "" || u.AuEmail.Contains(Email))
            && (AId == 0 || u.AId == AId);

            var dglist = this.IQueryable(predicate).Where(pag).ToList().Select(u => new
            {
                u.Id,
                u.AId,
                u.Article.Title,
                PName = this.IQueryable(r=>r.Id==u.PId).Select(r=>r.AuName).FirstOrDefault(),
                u.PId,
                u.Text,
                u.Ip,
                u.Agent,
                u.System,
                u.GeoPosition,
                u.UserHost,
                u.AuName,
                u.AuEmail,
                u.IsAduit,
                u.CreatorTime,
                HeadHref=u.AppUser == null ? u.HeadHref :u.AppUser.HeadHref,
            }).ToList();
            return new DataGrid { rows = dglist, total = pag.total };
        }
        #endregion

        #region 得到最新的count条回复信息
        public List<ReplyDto> GetRecentReview(int count)
        {
            return this.IQueryable().OrderByDescending(u => u.Id).Take(count).Select(u=>new {
                u.Article.Title,
                HeadHref=u.AppUser!=null?u.AppUser.HeadHref:u.HeadHref,
                u
            }).ToList().Select(r => new ReplyDto {
                Id=r.u.Id,
                AId=r.u.AId,
                ArticleTitle=r.Title,
                Text=r.u.Text,
                NickName=r.u.AuName,
                HeadHref=r.HeadHref
            }).ToList();
        } 
        #endregion
    }
}

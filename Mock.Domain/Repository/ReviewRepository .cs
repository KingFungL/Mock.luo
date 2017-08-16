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
    /// 仓储实现层 ReviewRepository
    /// </summary>]
    public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
    {
        public dynamic GetFormById(int Id)
        {
            return this.IQueryable(r => r.Id == Id).Select(r => new {
                r.Id,
                r.Text,
                r.Author,
                r.AuthorId,
                r.AuEmail
            }).FirstOrDefault();
        }

        public dynamic GetRecentReview(int count)
        {
           return this.IQueryable().OrderByDescending(u => u.Id).Take(count).Select(r => new {
               r.Id,
               r.Text,
               r.Author,
               r.AuthorId,
               r.AuEmail
           }).ToList();
        }
    }
}

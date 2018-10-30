using System;
using System.Linq;
using System.Linq.Expressions;
using Mock.Code.Extend;
using Mock.Code.Web;
using Mock.Data.AppModel;
using Mock.Data.Extensions;
using Mock.Data.Models;
using Mock.Data.Repository;
using Mock.Domain.Interface;

namespace Mock.Domain.Implementations
{
    /// <summary>
    /// 仓储实现层 GuestBookRepository
    /// </summary>]
    public class GuestBookRepository : RepositoryBase<GuestBook>, IGuestBookRepository
    {
        /// <summary>
        /// 根据留言标题与创建人邮箱查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pag">分页</param>
        /// <param name="search">标题/邮箱</param>
        /// <returns></returns>
        public DataGrid GetDataGrid(Expression<Func<GuestBook, bool>> predicate, PageDto pag, string search)
        {
            predicate = predicate.And(u => u.DeleteMark == false
             && (search == "" || u.AppUser.Email.Contains(search) || u.Text.Contains(search)));

            //var reviewList = base.IQueryable(u => u.PId== 0).Select(u => new
            //{
            //    u.PId,
            //    u.Id,
            //    u.AuName
            //}).ToList();

            var dglist = this.Queryable(predicate).Where(pag).Select(u => new
            {
                u = u,
                Avatar = u.AppUser == null ? u.Avatar : u.AppUser.Avatar,
            }).ToList().Select(r => new
            {
                r.Avatar,
                r.u.Id,
                //PName = reviewList.Where(s => s.Id == r.u.PId).Select(s => s.AuName).FirstOrDefault(),
                r.u.PId,
                r.u.Text,
                r.u.Ip,
                r.u.Agent,
                r.u.System,
                r.u.GeoPosition,
                r.u.UserHost,
                r.u.AuName,
                r.u.AuEmail,
                r.u.CreatorTime,
                r.u.IsAduit,
            }).ToList();

            return new DataGrid { Rows = dglist, Total = pag.Total };
        }
    }
}

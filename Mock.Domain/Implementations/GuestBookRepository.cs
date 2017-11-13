using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock.Code;
using System.Linq.Expressions;

namespace Mock.Domain
{
    /// <summary>
    /// 仓储实现层 GuestBookRepository
    /// </summary>]
    public class GuestBookRepository : RepositoryBase<GuestBook>, IGuestBookRepository
    {
        /// <summary>
        /// 根据留言标题与创建人邮箱查询
        /// </summary>
        /// <param name="pag">分页</param>
        /// <param name="param">标题/邮箱</param>
        /// <returns></returns>
        public DataGrid GetDataGrid(Expression<Func<GuestBook, bool>> predicate, Pagination pag, string search)
        {
            predicate = predicate.And(u => u.DeleteMark == false
             && (search == "" || u.AppUser.Email.Contains(search) || u.Text.Contains(search)));

            //var reviewList = base.IQueryable(u => u.PId== 0).Select(u => new
            //{
            //    u.PId,
            //    u.Id,
            //    u.AuName
            //}).ToList();

            var dglist = this.IQueryable(predicate).Where(pag).Select(u => new
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

            return new DataGrid { rows = dglist, total = pag.total };
        }
    }
}

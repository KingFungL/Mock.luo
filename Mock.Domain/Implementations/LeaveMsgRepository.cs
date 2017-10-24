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
    /// 仓储实现层 ReviewRepository
    /// </summary>]
    public class LeaveMsgRepository : RepositoryBase<LeaveMsg>, ILeaveMsgRepository
    {
       /// <summary>
       /// 根据留言标题与创建人邮件查询
       /// </summary>
       /// <param name="pag">分页</param>
       /// <param name="param">标题/邮件</param>
       /// <returns></returns>
        public DataGrid GetDataGrid(Pagination pag,string search)
        {
            Expression<Func<LeaveMsg, bool>> predicate = u => u.DeleteMark == false
            &&(search == ""||u.AppUser.Email.Contains(search)||u.LTitle.Contains(search));

            var dglist = this.IQueryable(predicate).Where(pag).Select(u => new
            {
                u.Id,
                u.LTitle,
                u.LContent,
                u.IsAduit,
                u.AppUser.LoginName,
                u.AppUser.Email,
                u.CreatorTime
            }).ToList();

            return new DataGrid { rows = dglist, total = pag.total };
        }
    }
}

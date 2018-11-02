using System;
using System.Linq.Expressions;
using Mock.Code.Web;
using Mock.Data.AppModel;
using Mock.Data.Models;
using Mock.Data.Repository;

namespace Mock.Domain.Interface
{
    public interface IGuestBookRepository : IRepositoryBase<GuestBook>
    {
        /// <summary>
        /// 根据留言标题与创建人邮箱查询留言内容
        /// </summary>
        /// <param name="pag">分页</param>
        /// <param name="param">标题/邮箱</param>
        /// <returns></returns>
        DataGrid GetDataGrid(Expression<Func<GuestBook, bool>> predicate,PageDto pag,string param);
        
    }
}

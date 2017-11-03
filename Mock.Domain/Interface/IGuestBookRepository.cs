using Mock.Code;
using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Domain
{
    public interface IGuestBookRepository : IRepositoryBase<GuestBook>
    {
        /// <summary>
        /// 根据留言标题与创建人邮件查询留言内容
        /// </summary>
        /// <param name="pag">分页</param>
        /// <param name="param">标题/邮件</param>
        /// <returns></returns>
        DataGrid GetDataGrid(Pagination pag,string param);
        
    }
}

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
    public interface ILeaveMsgRepository : IRepositoryBase<Review>
    {


        /// <summary>
        /// 
        /// </summary>
        /// <returns>DataGrid实体</returns>

        DataGrid GetDataGrid(Pagination pag);
        

    }
}

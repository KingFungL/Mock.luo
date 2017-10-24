using Mock.Code;
using Mock.Data;
using Mock.Data.Dto;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Domain
{
    public interface ILogInfoRepository : IRepositoryBase<LogInfo>
    {
        
        /// <summary>
        /// 根据条件得到日志的分页数据
        /// </summary>
        /// <param name="pag">分页条件</param>
        /// <returns>DataGrid实体</returns>
        DataGrid GetDataGrid(Pagination pag, string search);

    }
}

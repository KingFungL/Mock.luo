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

namespace Mock.Domain
{
    public class LogInfoRepository : RepositoryBase<LogInfo>, ILogInfoRepository
    {
        public DataGrid GetDataGrid(Pagination pag, string search)
        {
            Expression<Func<LogInfo, bool>> predicate = u => (search == "" || u.Message.Contains(search));

            var dglist = this.IQueryable(predicate).Where(pag).ToList();

            return new DataGrid { rows = dglist, total = pag.total };
        }
    }
}

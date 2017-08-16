using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock.Code;
using System.Data.Common;
using System.Linq.Expressions;

namespace Mock.Domain
{
    /// <summary>
    /// 仓储实现层 ItemsRepositroy
    /// </summary>]
    public class ItemsRepositroy : RepositoryBase<Items>, IItemsRepositroy
    {
        public DataGrid GetDataGrid(Pagination pag)
        {

            var dglist = this.IQueryable(u => u.DeleteMark == false).Where(pag).Select(u => new
            {
                u.Id,
                u.FullName,
                u.EnCode,
            }).ToList();

            return new DataGrid { rows = dglist, total = pag.total };

        }

        public dynamic GetFormById(int Id)
        {
            var d = this.IQueryable(u => u.Id == Id).Select(u => new
            {
                u.Id,
                u.FullName,
                u.EnCode,
            }).FirstOrDefault();
            return d;
        }


        public void Edit(Items Entity)
        {
            throw new NotImplementedException();
        }

        
    }
}

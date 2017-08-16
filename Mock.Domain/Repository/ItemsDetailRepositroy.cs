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
    /// 仓储实现层 ArticleRepositroy
    /// </summary>]
    public class ItemsDetailRepositroy : RepositoryBase<ItemsDetail>, IItemsDetailRepositroy
    {
        public void Edit(ItemsDetail Entity)
        {
            throw new NotImplementedException();
        }

        public DataGrid GetDataGrid(Pagination pag)
        {

            var dglist = this.IQueryable(u => u.DeleteMark == false).Where(pag).Select(u => new
            {
              
                u.Id
            }).ToList();

            return new DataGrid { rows = dglist, total = pag.total };

        }

        public dynamic GetFormById(int Id)
        {
            var d = this.IQueryable(u => u.Id == Id).Select(u => new
            {
                u.ItemCode,  
                u.Id,
                u.ItemName,
            }).FirstOrDefault();
            return d;
        }

        public dynamic GetItemDetailsFCode(string FCode)
        {
            return this.IQueryable(r=>r.Items.EnCode==FCode).Select(u => new {
               u.Id,u.ItemName,u.ItemCode,u.SortCode
            }).OrderBy(u=>u.SortCode).ToList();
        }
    }
}

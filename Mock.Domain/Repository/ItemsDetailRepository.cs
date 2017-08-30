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
    /// 仓储实现层 ArticleRepositroy
    /// </summary>]
    public class ItemsDetailRepository : RepositoryBase<ItemsDetail>, IItemsDetailRepository
    {

        public DataGrid GetDataGrid(Pagination pag,string ItemName,string EnCode)
        {
            Expression<Func<ItemsDetail, bool>> predicate = u => u.DeleteMark == false
            && (ItemName == "" || u.ItemName.Contains(ItemName))
            && (EnCode == "" || u.Items.EnCode == EnCode);
            var dglist = this.IQueryable(predicate).Where(pag).Select(u => new
            {
                u.Id,
                u.FId,
                u.ItemCode,
                u.ItemName,
                u.SortCode,
                u.IsEnableMark,
                u.Remark
            }).ToList();
            return new DataGrid { rows = dglist, total = pag.total };
        }

        public dynamic GetCombobox(string FCode)
        {
            return this.IQueryable(r=>r.Items.EnCode==FCode).OrderBy(u=>u.SortCode).Select(u => new {
               u.Id,u.ItemName,u.ItemCode
            }).ToList();
        }


    }
}

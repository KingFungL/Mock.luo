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

        public DataGrid GetDataGrid(Pagination pag, string ItemName, string EnCode)
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

        public List<TreeSelectModel> GetCombobox(string Encode)
        {
            List<TreeSelectModel> treeList = this.IQueryable(r => r.Items.EnCode == Encode).OrderBy(u => u.SortCode).ToList().Select(u => new TreeSelectModel
            {
                id = u.Id.ToString(),
                text = u.ItemName,
                parentId = "0"
            }).ToList();
            treeList.Insert(0, new TreeSelectModel { id ="-1", parentId = "0", text = "==请选择==" });
            return treeList;
        }


    }
}

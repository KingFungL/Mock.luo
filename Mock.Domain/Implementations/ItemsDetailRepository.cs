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
    /// 仓储实现层 ItemsDetailRepository
    /// </summary>]
    public class ItemsDetailRepository : RepositoryBase<ItemsDetail>, IItemsDetailRepository
    {
        #region 字典详情分页数据 DataGrid实体

        public DataGrid GetDataGrid(Pagination pag, string search, string EnCode)
        {
            Expression<Func<ItemsDetail, bool>> predicate = u => u.DeleteMark == false
            && (search == "" || u.ItemName.Contains(search)||u.ItemCode.Contains(search))
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
        #endregion

        #region 分类，根据items下的Encode获取ItemDetail的分类数据
        public List<TreeSelectModel> GetCombobox(string Encode)
        {
            List<TreeSelectModel> treeList = this.IQueryable(r => r.Items.EnCode == Encode).OrderBy(u => u.SortCode).ToList().Select(u => new TreeSelectModel
            {
                id = u.Id.ToString(),
                text = u.ItemName,
                parentId = "0"
            }).ToList();
            treeList.Insert(0, new TreeSelectModel { id = "-1", parentId = "0", text = "==请选择==" });
            return treeList;
        }
        #endregion

        #region 根据条件和主表Id获取分表列表数据
        public List<ItemsDetail> GetList(int itemId = 0, string keyword = "")
        {
            Expression<Func<ItemsDetail,bool>> expression = u=>u.DeleteMark==false;
            if (itemId != 0)
            {
                expression = expression.And(t => t.FId == itemId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.ItemName.Contains(keyword));
                expression = expression.Or(t => t.ItemCode.Contains(keyword));
            }
            return this.IQueryable(expression).OrderBy(t => t.SortCode).ToList();
        } 
        #endregion
    }
}

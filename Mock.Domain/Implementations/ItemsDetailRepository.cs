using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mock.Code.Extend;
using Mock.Code.Helper;
using Mock.Code.Web;
using Mock.Code.Web.Tree;
using Mock.Data.AppModel;
using Mock.Data.Extensions;
using Mock.Data.Models;
using Mock.Data.Repository;
using Mock.Domain.Interface;

namespace Mock.Domain.Implementations
{
    /// <summary>
    /// 仓储实现层 ItemsDetailRepository
    /// </summary>]
    public class ItemsDetailRepository : RepositoryBase<ItemsDetail>, IItemsDetailRepository
    {
        #region Constructor

        private readonly IRedisHelper _iRedisHelper;
        public ItemsDetailRepository(IRedisHelper iRedisHelper)
        {
            _iRedisHelper = iRedisHelper;
        }

        #endregion

        #region 字典详情分页数据 DataGrid实体

        public DataGrid GetDataGrid(PageDto pag, string search, string enCode)
        {
            Expression<Func<ItemsDetail, bool>> predicate = u => u.DeleteMark == false
            && (search == "" || u.ItemName.Contains(search) || u.ItemCode.Contains(search))
            && (enCode == "" || u.Items.EnCode == enCode);
            var dglist = this.Queryable(predicate).Where(pag).Select(u => new
            {
                u.Id,
                u.FId,
                u.ItemCode,
                u.ItemName,
                u.SortCode,
                u.IsEnableMark,
                u.Remark
            }).ToList();
            return new DataGrid { Rows = dglist, Total = pag.Total };
        }
        #endregion

        #region 分类，根据items下的Encode获取ItemDetail的分类数据
        public List<TreeSelectModel> GetCombobox(string encode)
        {
            return _iRedisHelper.UnitOfWork(string.Format(ConstHelper.ItemsDetailAll, "GetCombobox-"+ encode), () =>
               {
                   List<TreeSelectModel> treeList = this.Queryable(r => r.Items.EnCode == encode).OrderBy(u => u.SortCode).ToList().Select(u => new TreeSelectModel
                   {
                       Id = u.Id.ToString(),
                       Text = u.ItemName,
                       ParentId = "0",
                       Data = u.ItemCode
                   }).ToList();
                   treeList.Insert(0, new TreeSelectModel { Id = "-1", ParentId = "0", Text = "==请选择==" });
                   return treeList;
               });
        }
        #endregion

        #region 根据条件和主表Id获取分表列表数据
        public List<ItemsDetail> GetList(int itemId = 0, string keyword = "")
        {
            Expression<Func<ItemsDetail, bool>> expression = u => u.DeleteMark == false;
            if (itemId != 0)
            {
                expression = expression.And(t => t.FId == itemId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.ItemName.Contains(keyword));
                expression = expression.Or(t => t.ItemCode.Contains(keyword));
            }
            return this.Queryable(expression).OrderBy(t => t.SortCode).ToList();
        }
        #endregion
    }
}

using System.Collections.Generic;
using System.Linq;
using Mock.Code.Json;
using Mock.Code.Web.Tree;
using Mock.Code.Web.TreeGrid;
using Mock.Data.Models;
using Mock.Data.Repository;
using Mock.Domain.Interface;

namespace Mock.Domain.Implementations
{
    /// <summary>
    /// 仓储实现层 ItemsRepositroy
    /// </summary>]
    public class ItemsRepositroy : RepositoryBase<Items>, IItemsRepository
    {
        #region 字典分类jqGrid插件下树形表格数据
        public List<TreeGridModel> GetTreeGrid()
        {

            var dglist = this.Queryable(u => u.DeleteMark == false).OrderBy(u => u.SortCode).Select(u => new
            {
                u.Id,
                u.FullName,
                u.EnCode,
                u.PId,
                u.SortCode,
                u.Open,
                u.IsEnableMark,
                u.Remark
            }).ToList();

            var treeList = new List<TreeGridModel>();
            foreach (var item in dglist)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = dglist.Count(t => t.PId == item.Id) != 0;
                treeModel.Id = item.Id.ToString();
                treeModel.IsLeaf = hasChildren;
                treeModel.ParentId = item.PId.ToString();
                treeModel.Expanded = hasChildren;
                treeModel.EntityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return treeList;
        }
        #endregion

        #region 字典类别zTree树形数据
        public dynamic GetzTreeJson()
        {
            var itemsTreeJson = this.Queryable(u => u.DeleteMark == false && u.IsEnableMark == true).OrderBy(u => u.SortCode).Select(u => new
            {
                id = u.Id,
                pId = u.PId,
                name = u.FullName,
                open = u.Open,
                u.EnCode
            }).ToList();
            return itemsTreeJson;
        }
        #endregion

        #region 得到字典类别下拉树
        public List<TreeSelectModel> GetTreeJson()
        {
            List<TreeSelectModel> treeList = this.Queryable(u => u.DeleteMark == false && u.IsEnableMark == true).OrderBy(r => r.SortCode)
                                    .ToList().Select(u => new TreeSelectModel
                                    {
                                        Id = u.Id.ToString(),
                                        ParentId = u.PId.ToString(),
                                        Text = u.FullName,
                                        Data = new { u.EnCode }
                                    }).ToList();
            treeList.Insert(0, new TreeSelectModel { Id = "-1", Text = "==请选择==", ParentId = "0" });
            return treeList;
        } 
        #endregion
    }
}

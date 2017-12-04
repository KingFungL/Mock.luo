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
    public class ItemsRepositroy : RepositoryBase<Items>, IItemsRepository
    {
        #region 字典分类jqGrid插件下树形表格数据
        public List<TreeGridModel> GetTreeGrid()
        {

            var dglist = this.IQueryable(u => u.DeleteMark == false).OrderBy(u => u.SortCode).Select(u => new
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
                bool hasChildren = dglist.Count(t => t.PId == item.Id) == 0 ? false : true;
                treeModel.id = item.Id.ToString();
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.PId.ToString();
                treeModel.expanded = hasChildren;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return treeList;
        }
        #endregion

        #region 字典类别zTree树形数据
        public dynamic GetzTreeJson()
        {
            var itemsTreeJson = this.IQueryable(u => u.DeleteMark == false && u.IsEnableMark == true).OrderBy(u => u.SortCode).Select(u => new
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
            List<TreeSelectModel> treeList = this.IQueryable(u => u.DeleteMark == false && u.IsEnableMark == true).OrderBy(r => r.SortCode)
                                    .ToList().Select(u => new TreeSelectModel
                                    {
                                        id = u.Id.ToString(),
                                        parentId = u.PId.ToString(),
                                        text = u.FullName,
                                        data = new { u.EnCode }
                                    }).ToList();
            treeList.Insert(0, new TreeSelectModel { id = "-1", text = "==请选择==", parentId = "0" });
            return treeList;
        } 
        #endregion
    }
}

using System.Collections.Generic;
using Mock.Code.Web.Tree;
using Mock.Code.Web.TreeGrid;
using Mock.Data.Models;
using Mock.Data.Repository;

namespace Mock.Domain.Interface
{
    public interface IItemsRepository : IRepositoryBase<Items>
    {
        /// <summary>
        /// 字典分类jqGrid插件下树形表格数据
        /// </summary>
        /// <returns>jqGrid对应的treeGrid表格list</returns>
        List<TreeGridModel> GetTreeGrid();
        /// <summary>
        /// 字典类别zTree树形数据
        /// </summary>
        /// <returns>zTree要求的结构</returns>
        dynamic GetzTreeJson();
        /// <summary>
        /// 得到字典类别下拉树
        /// </summary>
        /// <returns>下拉树实体list</returns>
        List<TreeSelectModel> GetTreeJson();
    }
}

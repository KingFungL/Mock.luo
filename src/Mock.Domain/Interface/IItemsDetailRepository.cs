using System.Collections.Generic;
using Mock.Code.Web;
using Mock.Code.Web.Tree;
using Mock.Data.AppModel;
using Mock.Data.Models;
using Mock.Data.Repository;

namespace Mock.Domain.Interface
{
    public interface IItemsDetailRepository : IRepositoryBase<ItemsDetail>
    {
        /// <summary>
        /// 字典详情分页数据
        /// </summary>
        /// <param name="pag"></param>
        /// <returns></returns>
        DataGrid GetDataGrid(PageDto pag,string search,string enCode);

        /// <summary>
        /// 分类，根据items下的Encode获取ItemDetail的分类数据
        /// </summary>
        /// <param name="encode"></param>
        /// <returns></returns>
        List<TreeSelectModel> GetCombobox(string encode);
        /// <summary>
        /// 根据条件和主表Id获取分表列表数据
        /// </summary>
        /// <param name="itemId">主键</param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns></returns>

        List<ItemsDetail> GetList(int itemId = 0, string keyword = "");
    }
}

using Mock.Code;
using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Domain
{
    public interface IItemsDetailRepository : IRepositoryBase<ItemsDetail>
    {
        /// <summary>
        /// 字典详情分页数据
        /// </summary>
        /// <param name="pag"></param>
        /// <returns></returns>
        DataGrid GetDataGrid(Pagination pag,string ItemName,string EnCode);

        /// <summary>
        /// 分类，根据items下的Encode获取ItemDetail的分类数据
        /// </summary>
        /// <param name="Encode"></param>
        /// <returns></returns>
        List<TreeSelectModel> GetCombobox(string Encode);
        /// <summary>
        /// 根据条件和主表Id获取分表列表数据
        /// </summary>
        /// <param name="itemId">主键</param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns></returns>

        List<ItemsDetail> GetList(int itemId = 0, string keyword = "");
    }
}

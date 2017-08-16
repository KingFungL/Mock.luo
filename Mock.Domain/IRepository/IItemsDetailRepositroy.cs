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
    public interface IItemsDetailRepositroy : IRepositoryBase<ItemsDetail>
    {
        dynamic GetFormById(int Id);
        DataGrid GetDataGrid(Pagination pag);
         void Edit(ItemsDetail Entity);

        /// <summary>
        /// 分类，根据itemsa下的ItemCode获取ItemDetail的分类数据
        /// </summary>
        /// <param name="FCode"></param>
        /// <returns></returns>
        dynamic GetItemDetailsFCode(string FCode);
    }
}

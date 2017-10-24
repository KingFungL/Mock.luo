using Mock.Code;
using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Domain
{
    public interface IPointArticleRepository : IRepositoryBase<PointArticle>
    {

        /// <summary>
        /// 根据文章id得到点赞人信息 
        /// </summary>
        /// <param name="ArticeId">文章主键</param>
        /// <returns>DataGrid实体</returns>
        DataGrid GetDataGrid(int ArticeId);
    }
}

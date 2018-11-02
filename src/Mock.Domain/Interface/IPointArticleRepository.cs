using Mock.Data.AppModel;
using Mock.Data.Models;
using Mock.Data.Repository;

namespace Mock.Domain.Interface
{
    public interface IPointArticleRepository : IRepositoryBase<PointArticle>
    {

        /// <summary>
        /// 根据文章id得到点赞人信息 
        /// </summary>
        /// <param name="articeId">文章主键</param>
        /// <returns>DataGrid实体</returns>
        DataGrid GetDataGrid(int articeId);
    }
}

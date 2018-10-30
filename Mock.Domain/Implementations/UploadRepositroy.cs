using System.Linq;
using Mock.Data.Models;
using Mock.Data.Repository;
using Mock.Domain.Interface;

namespace Mock.Domain.Implementations
{
    /// <summary>
    /// 仓储实现层 UploadRepository
    /// </summary>]
    public class UploadRepositroy : RepositoryBase<Upload>, IUploadRepository
    {
        public dynamic GetFormById(int id)
        {
            var d = this.Queryable(u => u.Id == id).Select(u => new
            {
                u.Id
            }).FirstOrDefault();
            return d;
        }
    }
}

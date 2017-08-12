using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Domain
{
    /// <summary>
    /// 仓储实现层 UploadRepository
    /// </summary>]
    public class UploadRepositroy : RepositoryBase<Upload>, IUploadRepository
    {
        public dynamic GetFormById(int Id)
        {
            var d = this.IQueryable(u => u.Id == Id).Select(u => new
            {
                u.Id,
                u.AddTime,
                u.UserName,
                FileSize = u.UploadEntry.Select(v => v.FileSize),
                FileName = u.UploadEntry.Select(v => v.FileName),
                Url = u.UploadEntry.Select(v => v.Url),
            }).FirstOrDefault();
            return d;
        }
    }
}

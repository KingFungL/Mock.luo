using Mock.Data;
using Mock.Domain.IRepository;
using Mock.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mock.Luo.Areas.Models
{
    public class UploadApp
    {
        public dynamic GetFormById(int Id)
        {
            IUploadRepository service = new UploadRepository();
            var d = service.IQueryable(u => u.Id == Id).Select(u => new
            {
                u.Id,
                u.AddTime,
                u.UserName,
                FileSize = u.UploadEntries.Select(v => v.FileSize),
                FileName = u.UploadEntries.Select(v => v.FileName),
                Url = u.UploadEntries.Select(v => v.Url),
            }).FirstOrDefault();
            return d;
        }
    }
}
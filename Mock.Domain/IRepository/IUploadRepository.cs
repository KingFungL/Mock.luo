using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Domain
{
    public interface IUploadRepository : IRepositoryBase<Upload>
    {
         dynamic GetFormById(int Id);
    }
}

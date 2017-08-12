using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data
{
    public interface IModificationAudited
    {
        int? Id { get; set; }
        int?  LastModifyUserId { get; set; }
        DateTime?  LastModifyTime { get; set; }
    }
}

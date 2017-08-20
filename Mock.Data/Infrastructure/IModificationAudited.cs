using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data
{
    public interface IModificationAudited : IAduited
    {
        int? LastModifyUserId { get; set; }
        DateTime? LastModifyTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data
{
    public interface ICreationAudited : IAduited
    {
        int? CreatorUserId { get; set; }
        DateTime? CreatorTime { get; set; }
        bool? DeleteMark { get; set; }
    }
}

using System;

namespace Mock.Data.Infrastructure
{
    public interface IModificationAudited : IAduited
    {
        int? LastModifyUserId { get; set; }
        DateTime? LastModifyTime { get; set; }
    }
}

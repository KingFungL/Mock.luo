using System;

namespace Mock.Data.Infrastructure
{
    public interface ICreationAudited : IAduited
    {
        int? CreatorUserId { get; set; }
        DateTime? CreatorTime { get; set; }
        bool? DeleteMark { get; set; }
    }
}

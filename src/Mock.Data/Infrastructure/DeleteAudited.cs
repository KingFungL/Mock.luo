using System;

namespace Mock.Data.Infrastructure
{
    public class DeleteAudited : IDeleteAudited
    {
        public int Id { get; set; }
        /// <summary>
        /// 逻辑删除标记
        /// </summary>
        public bool? DeleteMark { get; set; }

        /// <summary>
        /// 删除实体的用户
        /// </summary>
        public int? DeleteUserId { get; set; }

        /// <summary>
        /// 删除实体时间
        /// </summary>
        public DateTime? DeleteTime { get; set; }
    }
}



namespace Mock.Code.Web
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class PageDto
    {
        /// <summary>
        /// 每页行数
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// 偏移值  当limit为10，即每页为10条记录，offset为0，偏移0，当为第二页时，offset就为10,就偏移10.
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// 排序类型，desc,asc
        /// </summary>
        public string Order { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }


    }
}



namespace Mock.Code
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// 每页行数
        /// </summary>
        public int limit { get; set; }
        /// <summary>
        /// 偏移值  当limit为10，即每页为10条记录，offset为0，偏移0，当为第二页时，offset就为10,就偏移10.
        /// </summary>
        public int offset { get; set; }
        /// <summary>
        /// 排序类型，desc,asc
        /// </summary>
        public string order { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string sort { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int total { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Code
{
    /// <summary>
    /// 日志消息
    /// </summary>
    public class LogMessage
    {
        public int LogId { get; set; }
        public int? CategoryId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }

        public int? OperateUserId { get; set; }

        public string OperateAccount { get; set; }

        public string OperateType { get; set; }

        public int? ModuleId { get; set; }
        public string Module { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 浏览器
        /// </summary>
        public string Browser { get; set; }

        public int ExecuteResult { get; set; }
        public string ExecuteResultJson { get; set; }
        
        public bool DeleteMark { get; set; }

        public string DeleteTime { get; set; }

        public string Url { get; set; }
        public string Class { get; set; }
        public string Content { get; set; }

        public string ExceptionInfo { get; set; }

    }
}

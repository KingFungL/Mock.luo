using System;
using System.ComponentModel.DataAnnotations;

namespace Mock.Code.Log
{
    /// <summary>
    /// 日志消息
    /// </summary>
    public class LogMessage
    {

        [Key]
        public int LogId { get; set; }
        public int CategoryId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }

        public int OperateUserId { get; set; }
        [StringLength(50)]

        public string OperateAccount { get; set; }

        [StringLength(50)]
        public string OperateType { get; set; }

        public int ModuleId { get; set; }
        [StringLength(50)]
        public string Module { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        [StringLength(50)]
        public string IpAddress { get; set; }
        /// <summary>
        /// 主机
        /// </summary>
        [StringLength(50)]
        public string Host { get; set; }
        /// <summary>
        /// 浏览器
        /// </summary>
        [StringLength(50)]
        public string Browser { get; set; }

        public int ExecuteResult { get; set; }
        [StringLength(4000)]
        public string ExecuteResultJson { get; set; }
        [StringLength(500)]
        public string MethodName { get; set; }
        [StringLength(500)]
        public string Exception { get; set; }
        [StringLength(500)]
        public string ServiceName { get; set; }
        [StringLength(4000)]
        public string ExceptionSource { get;  set; }
        [StringLength(4000)]
        public string ExceptionRemark { get;  set; }
        [StringLength(1024)]
        public string Parameters { get; set; }
    }
}

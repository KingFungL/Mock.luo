

namespace Mock.Code.Web
{
    public class AjaxResult
    {
        /// <summary>
        /// 操作结果类型
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 获取 消息内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 获取 返回数据
        /// </summary>
        public object Data { get; set; }

        public static AjaxResult Info(string message, object data, string state)
        {
            return new AjaxResult { State = state, Message = message, Data = data };
        }

        public static AjaxResult Success(string message, object data = null)
        {
            return Info(message, data, ResultType.Success.ToString());
        }
        public static AjaxResult Error(string message, object data = null)
        {
            return Info(message, data, ResultType.Error.ToString());
        }

    }
    /// <summary>
    /// 表示 ajax 操作结果类型的枚举
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// 警告结果类型
        /// </summary>
        Warning,
        /// <summary>
        /// 成功结果类型
        /// </summary>
        Success,
        /// <summary>
        /// 异常结果类型
        /// </summary>
        Error,
        /// <summary>
        /// 消息结果类型
        /// </summary>
        Info,
        /// <summary>
        /// 未登录
        /// </summary>
        Nologin,
        /// <summary>
        /// 没有权限
        /// </summary>
        Nopermission

    }
}



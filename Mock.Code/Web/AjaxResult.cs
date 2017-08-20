

namespace Mock.Code
{
    public class AjaxResult
    {
        /// <summary>
        /// 操作结果类型
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 获取 消息内容
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 获取 返回数据
        /// </summary>
        public object data { get; set; }

        public static AjaxResult Info(string message, object data, string state)
        {
            return new AjaxResult { state = state, message = message, data = data };
        }

        public static AjaxResult Success(string message, object data = null)
        {
            return Info(message, data, ResultType.success.ToString());
        }
        public static AjaxResult Error(string message, object data = null)
        {
            return Info(message, data, ResultType.error.ToString());
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
        warning,
        /// <summary>
        /// 成功结果类型
        /// </summary>
        success,
        /// <summary>
        /// 异常结果类型
        /// </summary>
        error,
        /// <summary>
        /// 消息结果类型
        /// </summary>
        info,
        /// <summary>
        /// 未登录
        /// </summary>
        nologin,
        /// <summary>
        /// 没有权限
        /// </summary>
        nopermission

    }
}



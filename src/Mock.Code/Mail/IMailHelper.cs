namespace Mock.Code.Mail
{
    public interface IMailHelper
    {
        /// <summary>
        /// 同步发送邮箱
        /// </summary>
        /// <param name="to">收件人邮箱地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="encoding">编码</param>
        /// <param name="isBodyHtml">是否Html</param>
        /// <param name="enableSsl">是否SSL加密连接</param>
        /// <returns>是否成功</returns>
        bool Send(string to, string subject, string body, string encoding = "UTF-8", bool isBodyHtml = true, bool enableSsl = false);
        /// <summary>
        /// 异步发送邮箱 独立线程
        /// </summary>
        /// <param name="to">邮箱接收人</param>
        /// <param name="title">邮箱标题</param>
        /// <param name="body">邮箱内容</param>
        /// <param name="port">端口号</param>
        /// <returns></returns>
        void SendByThread(string to, string title, string body, int port = 25);
    }
}

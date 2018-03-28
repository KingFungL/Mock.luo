using Mock.Code;
using Mock.Data;
using Mock.Domain;
using System;
using System.Web.Mvc;

namespace Mock.Luo.Generic.Filters
{
    public class HandlerErrorAttribute : HandleErrorAttribute
    {
        private ILogInfoRepository _iLogRepository { get; set; }
        private IMailHelper _mailHelper { get; set; }
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = 200;


            ContentResult conResult = new ContentResult { Content = new AjaxResult { state = ResultType.error.ToString(), message = context.Exception.Message }.ToJson() };


            context.Result = conResult;

            this.WriteLog(context);
        }
        private void WriteLog(ExceptionContext context)
        {
            if (context == null)
                return;
            var log = LogFactory.GetLogger(context.Controller.ToString());

            LogMessage logMessage = new LogMessage
            {
                CategoryId = 4,
                OperateType = EnumAttribute.GetDescription(DbLogType.Exception),
                ExecuteResult = -1,
                Class = context.Controller.ToString(),
                OperateAccount = OperatorProvider.Provider.CurrentUser.LoginName + "（" + OperatorProvider.Provider.CurrentUser.UserId + "）"
            };
            Exception Error = context.Exception;
            if (Error.InnerException == null)
            {
                logMessage.ExceptionInfo = Error.Message;
            }
            else
            {
                logMessage.ExceptionInfo = Error.InnerException.Message;
            }
            string strMessage = new LogFormat().ExceptionFormat(logMessage);

            logMessage.ExecuteResultJson = strMessage;

            _iLogRepository.LogError(logMessage, "错误日志");


            SendMail(strMessage);

        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        private void SendMail(string body)
        {
            bool ErrorToMail = Configs.GetValue("ErrorToMail").ToBool();
            if (ErrorToMail == true)
            {
                string SystemName = Configs.GetValue("SystemName");//系统名称
                _mailHelper.SendByThread("710277267@qq.com", SystemName + " - 发生异常", body.Replace("-", ""));
            }
        }


    }
}
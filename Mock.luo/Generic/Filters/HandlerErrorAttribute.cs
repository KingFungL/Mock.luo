using System;
using System.Web.Mvc;
using Mock.Code.Attribute;
using Mock.Code.Configs;
using Mock.Code.Extend;
using Mock.Code.Json;
using Mock.Code.Log;
using Mock.Code.Mail;
using Mock.Code.Web;
using Mock.Data.AppModel;
using Mock.Domain.Interface;

namespace Mock.luo.Generic.Filters
{
    public class HandlerErrorAttribute : HandleErrorAttribute
    {
        private ILogInfoRepository LogRepository { get; set; }
        private IMailHelper MailHelper { get; set; }
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = 200;


            ContentResult conResult = new ContentResult { Content = new AjaxResult { State = ResultType.Error.ToString(), Message = context.Exception.Message }.ToJson() };


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
            Exception error = context.Exception;
            if (error.InnerException == null)
            {
                logMessage.ExceptionInfo = error.Message;
            }
            else
            {
                logMessage.ExceptionInfo = error.InnerException.Message;
            }
            string strMessage = new LogFormat().ExceptionFormat(logMessage);

            logMessage.ExecuteResultJson = strMessage;

            LogRepository.LogError(logMessage, "错误日志");


            SendMail(strMessage);

        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        private void SendMail(string body)
        {
            bool errorToMail = Configs.GetValue("ErrorToMail").ToBool();
            if (errorToMail == true)
            {
                string systemName = Configs.GetValue("SystemName");//系统名称
                MailHelper.SendByThread("710277267@qq.com", systemName + " - 发生异常", body.Replace("-", ""));
            }
        }


    }
}
using Mock.Code.Attribute;
using Mock.Code.Configs;
using Mock.Code.Extend;
using Mock.Code.Json;
using Mock.Code.Log;
using Mock.Code.Mail;
using Mock.Code.Web;
using Mock.Domain.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Mock.Luo.Generic.Filters
{
    public class HandlerErrorAttribute : HandleErrorAttribute
    {
        public ILogMessageRepository LogRepository { get; set; }
        public IMailHelper MailHelper { get; set; }
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

            var paramsForm = context.RequestContext.HttpContext.Request.Form;
            Exception error = context.Exception;

            LogMessage logMessage = new LogMessage
            {
                CategoryId = (int)DbLogType.Exception,
                OperateType = EnumAttribute.GetDescription(DbLogType.Exception),
                ExecuteResult = -1,
                MethodName = (string)context.RouteData.Values["action"],
                Parameters = ConvertArgumentsToJson(paramsForm.ToDictionary()),
                Exception = error.InnerException == null ? error.Message : error.InnerException.Message,
                ExceptionSource = error.Source,
                ExceptionRemark = error.StackTrace,
            };

            LogRepository.LogError(logMessage, "错误日志");
#if !DEBUG
            SendMail(logMessage.ExecuteResultJson);
#endif
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        private void SendMail(string body)
        {
            bool errorToMail = Configs.GetValue("ErrorToMail").ToBool();
            if (errorToMail == true)
            {
                string softName = Configs.GetValue("SoftName");//系统名称
                MailHelper.SendByThread("710277267@qq.com", softName + " - 发生异常", body.Replace("-", "")?.Replace("\r\n", "</br>"));
            }
        }

        private string ConvertArgumentsToJson(IDictionary<string, object> arguments)
        {
            try
            {
                if (arguments.IsNullOrEmpty())
                {
                    return "{}";
                }

                var dictionary = new Dictionary<string, object>();

                foreach (var argument in arguments)
                {
                    if (argument.Value == null)
                    {
                        dictionary[argument.Key] = null;
                    }
                    else
                    {
                        dictionary[argument.Key] = argument.Value;
                    }
                }

                return JsonConvert.SerializeObject(dictionary);
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger("ConvertArgumentsToJson").Warn(ex.ToString());
                return "{}";
            }
        }

    }
}
using log4net.Core;
using log4net.Layout.Pattern;
using log4net.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Code
{
    public class ActionConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (Option != null)
            {
                // Write the value for the specified key
                WriteObject(writer, loggingEvent.Repository, LookupProperty(Option, loggingEvent));
            }
            else
            {
                // Write all the key value pairs
                WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
            }


            #region MyRegion
            //if (loggingEvent == null)
            //{
            //    writer.Write(SystemInfo.NullText);
            //}
            //else
            //{
            //    LogMessage logMessage = loggingEvent.MessageObject as LogMessage;

            //    if (logMessage == null)
            //    {
            //        writer.Write(SystemInfo.NullText);
            //    }
            //    else
            //    {
            //        switch (this.Option.ToLower())
            //        {
            //            case "CategoryId":
            //                writer.Write(logMessage.CategoryId);
            //                break;
            //            case "OperateTime":
            //                writer.Write(logMessage.OperateTime);
            //                break;
            //            case "OperateUserId":
            //                writer.Write(logMessage.OperateUserId);
            //                break;
            //            case "OperateAccount":
            //                writer.Write(logMessage.OperateAccount);
            //                break;
            //            case "OperateType":
            //                writer.Write(logMessage.OperateType);
            //                break;
            //            case "ModuleId":
            //                writer.Write(logMessage.ModuleId);
            //                break;
            //            case "Module":
            //                writer.Write(logMessage.Module);
            //                break;
            //            case "IpAddress":
            //                writer.Write(logMessage.IpAddress);
            //                break;
            //            case "Host":
            //                writer.Write(logMessage.Host);
            //                break;
            //            case "Browser":
            //                writer.Write(logMessage.Browser);
            //                break;
            //            case "ExecuteResult":
            //                writer.Write(logMessage.ExecuteResult);
            //                break;
            //            case "ExecuteResultJson":
            //                writer.Write(logMessage.ExecuteResultJson);
            //                break;
            //            case "DeleteMark":
            //                writer.Write(logMessage.DeleteMark);
            //                break;
            //            default:
            //                writer.Write(SystemInfo.NullText);
            //                break;
            //        }

            //    } 
            //}
            #endregion
        }

        /// <summary>
        /// 通过反射获取传入的日志对象的某个属性的值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="loggingEvent"></param>
        /// <returns></returns>
        private static object LookupProperty(string property, LoggingEvent loggingEvent)
        {
            object messageObject = loggingEvent.MessageObject;
            PropertyInfo propertyInfo = messageObject.GetType().GetProperty(property);

            object propertyValue = propertyInfo != null ? propertyInfo.GetValue(messageObject, null) : string.Empty;
            return propertyValue;
        }
    }
}
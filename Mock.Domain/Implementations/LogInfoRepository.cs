using System;
using System.Web;
using Mock.Code.Log;
using Mock.Code.Net;
using Mock.Code.Web;
using Mock.Data.AppModel;
using Mock.Domain.Interface;

namespace Mock.Domain.Implementations
{
    public class LogInfoRepository :  ILogInfoRepository
    {
        public DataGrid GetDataGrid(PageDto pag, string search)
        {
            //Expression<Func<LogInfo, bool>> predicate = u => (search == "" || u.Message.Contains(search));

            //var dglist = this.IQueryable(predicate).Where(pag).ToList();

            return new DataGrid { Rows = "", Total = pag.Total };
        }


        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logEntity">对象</param>
        public void LogError(LogMessage logEntity,string logName)
        {
            logEntity.Url = HttpContext.Current.Request.RawUrl;
            logEntity.OperateTime = DateTime.Now;
            logEntity.DeleteMark = false;
            logEntity.IpAddress = Net.Ip;
            logEntity.Host = Net.Host;
            logEntity.Browser = Net.Browser;
            Log log = LogFactory.GetLogger(logName);
            log.Error(logEntity);

        }
    }
}

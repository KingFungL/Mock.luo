using Mock.Code.Log;
using Mock.Code.Net;
using Mock.Code.Web;
using Mock.Data.AppModel;
using Mock.Domain.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Mock.Data.Extensions;
using Mock.Data.Repository;

namespace Mock.Domain.Implementations
{
    public class LogMessageRepository : RepositoryBase<LogMessage>, ILogMessageRepository
    {
        public DataGrid GetDataGrid(PageDto pag, string search)
        {
            Expression<Func<LogMessage, bool>> predicate = u => (search == "" || u.ExecuteResultJson.Contains(search));

            var dglist = this.Queryable(predicate).Where(pag).ToList();

            return new DataGrid { Rows = dglist, Total = pag.Total };
        }


        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logEntity">对象</param>
        public void LogError(LogMessage logEntity, string logName)
        {
            logEntity.ServiceName = HttpContext.Current.Request.RawUrl;
            logEntity.OperateTime = DateTime.Now;
            logEntity.IpAddress = Net.Ip;
            logEntity.Host = Net.Host;
            logEntity.Browser = Net.Browser;
            logEntity.OperateAccount = OperatorProvider.Provider.CurrentUser?.LoginName + "（" +OperatorProvider.Provider.CurrentUser?.UserId + "）";

            logEntity.ExecuteResultJson =  new LogFormat().ExceptionFormat(logEntity);
            Log log = LogFactory.GetLogger(logName);
            log.Error(logEntity);

        }
    }
}

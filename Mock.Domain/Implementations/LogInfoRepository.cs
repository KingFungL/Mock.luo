using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock.Code;
using System.Linq.Expressions;
using Mock.Data.Dto;
using System.Web;

namespace Mock.Domain
{
    public class LogInfoRepository :  ILogInfoRepository
    {
        public DataGrid GetDataGrid(Pagination pag, string search)
        {
            //Expression<Func<LogInfo, bool>> predicate = u => (search == "" || u.Message.Contains(search));

            //var dglist = this.IQueryable(predicate).Where(pag).ToList();

            return new DataGrid { rows = "", total = pag.total };
        }


        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logEntity">对象</param>
        public void LogError(LogMessage logEntity,string LogName)
        {
            logEntity.Url = HttpContext.Current.Request.RawUrl;
            logEntity.OperateTime = DateTime.Now;
            logEntity.DeleteMark = false;
            logEntity.IpAddress = Net.Ip;
            logEntity.Host = Net.Host;
            logEntity.Browser = Net.Browser;
            Log log = LogFactory.GetLogger(LogName);
            log.Error(logEntity);

        }
    }
}

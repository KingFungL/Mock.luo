using System.Web.Mvc;
using Mock.Code.Web;
using Mock.Domain.Interface;
using Mock.Luo.Controllers;

namespace Mock.Luo.Areas.Plat.Controllers
{
    public class LogInfoController : BaseController
    {
        // GET: Plat/LogInfo
        ILogInfoRepository _service;
        public LogInfoController(ILogInfoRepository service) 
        {
            this._service = service;
        }



        public ActionResult GetDataGrid(PageDto pag,string search="")
        {
            return Result(_service.GetDataGrid(pag, search));
        }
    }
}
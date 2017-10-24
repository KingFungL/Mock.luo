using Mock.Code;
using Mock.Domain;
using Mock.Luo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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



        public ActionResult GetDataGrid(Pagination pag,string search="")
        {
            return Result(_service.GetDataGrid(pag, search));
        }
    }
}
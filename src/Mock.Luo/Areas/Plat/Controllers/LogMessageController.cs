using Mock.Code.Json;
using Mock.Code.Web;
using Mock.Domain.Interface;
using Mock.Luo.Controllers;
using System.Web.Mvc;

namespace Mock.Luo.Areas.Plat.Controllers
{
    public class LogMessageController : BaseController
    {
        // GET: Plat/LogMessage
        private readonly ILogMessageRepository _logMessageRepository;
        public LogMessageController(ILogMessageRepository logMessageRepository)
        {
            this._logMessageRepository = logMessageRepository;
        }
        public ActionResult GetDataGrid(PageDto pag, string search = "")
        {
            return Result(_logMessageRepository.GetDataGrid(pag, search));
        }

        public override ActionResult Detail(int id)
        {
            return View(_logMessageRepository.FindEntity(id));
        }

    }
}
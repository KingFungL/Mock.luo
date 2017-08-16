using Mock.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mock.Luo.Controllers
{
    public abstract class BaseController : Controller
    {
        OperatorProvider op = OperatorProvider.Provider;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }
        protected virtual ActionResult Success()
        {
            return Success("操作完成!");
        }
        protected virtual ActionResult Success(string message)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message }.ToJson());
        }
        protected virtual ActionResult Success(string message, object data)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message, data = data }.ToJson());
        }
        protected virtual ActionResult Error(string message)
        {
            return Content(new AjaxResult { state = ResultType.error.ToString(), message = message }.ToJson());
        }
        protected virtual ActionResult Error(ResultType state, string message, object data)
        {
            return Content(new AjaxResult { state = state.ToString(), message = message, data = data }.ToJson());
        }

        protected virtual ActionResult Result(object data)
        {
            return Content(JsonHelper.SerializeObject(data));
        }

    }
}

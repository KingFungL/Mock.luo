using System.Linq;
using System.Web.Mvc;
using Mock.Code.Helper;
using Mock.Code.Json;
using Mock.Code.Web;
using Mock.Data.AppModel;

namespace Mock.luo.Controllers
{
    public abstract class BaseController : Controller
    {
        public OperatorProvider Op = OperatorProvider.Provider;
        //[HandlerAuthorize]

        public virtual ActionResult Index()
        {
            return View();
        }
        public virtual ActionResult Detail(int id)
        {
            return View();
        }
        protected virtual ActionResult Success()
        {
            return Success("操作完成!");
        }
        protected virtual ActionResult Error()
        {
            return Error("操作失败！");
        }
        protected virtual ActionResult Success(string message)
        {
            return Content(new AjaxResult { State = ResultType.Success.ToString(), Message = message }.ToJson());
        }
        protected virtual ActionResult Success(string message, object data)
        {
            return Content(new AjaxResult { State = ResultType.Success.ToString(), Message = message, Data = data }.ToJson());
        }
        protected virtual ActionResult Error(string message)
        {
            return Content(new AjaxResult { State = ResultType.Error.ToString(), Message = message }.ToJson());
        }
        protected virtual ActionResult Error(ResultType state, string message, object data)
        {
            return Content(new AjaxResult { State = state.ToString(), Message = message, Data = data }.ToJson());
        }
        protected virtual ActionResult Error(ModelStateDictionary modelState)
        {
            return Error(modelState.Values.Where(u => u.Errors.Count > 0).FirstOrDefault().Errors[0].ErrorMessage);
        }

        /// <summary>
        /// 简化返回json对象的包裹
        /// </summary>
        /// <param name="data">object对象</param>
        /// <returns></returns>
        protected virtual ActionResult Result(object data)
        {
            return Content(JsonHelper.SerializeObject(data));
        }

    }
}

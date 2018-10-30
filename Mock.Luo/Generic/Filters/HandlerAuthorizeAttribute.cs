using System.Net;
using System.Web.Mvc;
using Mock.Code.Attribute;
using Mock.Code.Web;
using Mock.Data.AppModel;
using Mock.Domain.Interface;

namespace Mock.luo.Generic.Filters
{
    /// <summary>
    ///描述：（权限认证+安全）拦截组件
    /// </summary>
    public class HandlerAuthorizeAttribute : ActionFilterAttribute
    {


        public IAppModuleRepository AppModuleRepository { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (OperatorProvider.Provider.CurrentUser.IsAdmin == true)
            {
                return;
            }
            //当有skip验证时，去除验证登录
            bool skipignore = filterContext.ActionDescriptor.GetCustomAttributes(typeof(SkipAttribute), false).Length == 1
                                       || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(SkipAttribute), false);
            if (skipignore == true) return;

            if (!this.ActionAuthorize(filterContext))
            {
                #region 是否具有HttpPost/HttpGet请求验证
                var isAjaxRequest = filterContext.HttpContext.Request.IsAjaxRequest();
                #endregion
                //如果不是ajax请求，就重定向到一个权限不足的页面。
                if (!isAjaxRequest)
                {
                    WebHelper.WriteCookie("mock_authorize_error", "nopermission");//无权限
                    filterContext.Result = new RedirectResult("~/Login/Default");
                }
                else
                {
                    AjaxResult amm = AjaxResult.Info("很抱歉！您的权限不足，访问被拒绝！", "", ResultType.Nopermission.ToString());
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;//无权限状态码  
                    filterContext.Result = new JsonResult { Data = amm, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                return;
            }
        }
        private bool ActionAuthorize(ActionExecutingContext filterContext)
        {
            var operatorProvider = OperatorProvider.Provider.CurrentUser;

            //权限认证，待完成2017-6-7 updated 2017-12-3

            #region  无法有效地判断前端请求的地址与后台存储的LinkUrl相同
            //区域
            string area = string.Empty;
            //控制器
            string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //方法
            string action = filterContext.ActionDescriptor.ActionName;

            var routeData = filterContext.RequestContext.RouteData;
            if (routeData.DataTokens["area"] != null)
            {
                area = routeData.DataTokens["area"].ToString();
            }
            #endregion
            
            //自己动手拼装
            string requestUrl = "/" + controller + "/" + action;
            if (!string.IsNullOrEmpty(area))
            {
                requestUrl = "/" + area + requestUrl;
            }
            ///Plat/AppUser/Form/0  也会报权限出错
            //string currentUrl = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
            return AppModuleRepository.ActionAuthorize(SystemInfo.CurrentUserId, SystemInfo.CurrentModuleId, requestUrl);

        }
    }
}
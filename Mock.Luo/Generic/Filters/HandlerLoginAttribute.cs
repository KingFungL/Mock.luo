using Mock.Code;
using System;
using System.Web.Mvc;
using Mock.Data;
using System.Text;
using System.Web;

namespace Mock.Luo.Generic.Filters
{
    public class HandlerLoginAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            //当有skip验证时，去除验证登录
            bool skipignore = !filterContext.ActionDescriptor.IsAttributeDefined<SkipAttribute>(false)
                                       && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(SkipAttribute), false);
            if (skipignore == false) return;

            OperatorProvider op = OperatorProvider.Provider;
            //判断用户是否登录
            if (op.CurrentUser == null)
            {
                StringBuilder sbScript = new StringBuilder();
                sbScript.Append("<script type='text/javascript'>alert('请选登录!');top.location.href='/Login/Index?msg=noLogin'</script>");
                filterContext.Result = new ContentResult() { Content = sbScript.ToString() };
            }
            else
            {
                bool loginOnce = Configs.GetValue("loginOnce").ToBoolean();

                if (loginOnce)
                {
                    //当前登录信息被覆盖，说明存在多个浏览器登录的情况
                    string userid = op.CurrentUser.UserId.ToString();
                    if (HttpRuntime.Cache[userid]?.ToString() != filterContext.HttpContext.Session.SessionID)
                    {
                        //清空Session
                        filterContext.HttpContext.Session.Remove(userid);
                        op.RemoveCurrent();
                        //跳转强制下线界面
                        this.HandleUnauthorizedRequest(filterContext);
                    }
                }

            }
        }

        /// <summary>
        ///   当重写验证返回为false时进入该重写方法
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            if (filterContext.HttpContext.Request.Url != null)
            {
                filterContext.Result = new RedirectResult("/Error/ReturnToLogin");
            }
        }
    }
}

using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.SessionState;
using Mock.Code;
using Mock.Code.Configs;
using Mock.Code.Extend;
using Mock.Code.Json;
using Mock.Code.Net;
using Mock.Code.Security;
using Mock.Code.Web;

namespace Mock.Data.AppModel
{
    public class OperatorProvider
    {
        #region 基础 当前上下文对象
        /// </summary
        /// <summary>
        /// 当前上下文对象
        /// </summary>
        public static OperatorProvider Provider
        {
            get
            {
                OperatorProvider opContext = CallContext.GetData(typeof(OperatorProvider).Name) as OperatorProvider;
                if (opContext == null)
                {
                    opContext = new OperatorProvider();
                    CallContext.SetData(typeof(OperatorProvider).Name, opContext);
                }
                return opContext;
            }
        }
        /// <summary>
        /// 登录用户的key
        /// </summary>
        private string _loginUserKey = "mock_luo_loginuserkey_2017";
        /// <summary>
        /// 配置信息session或cookies
        /// </summary>
        private string _loginProvider = Configs.GetValue("LoginProvider");
        /// <summary>
        /// 单点登录标志
        /// </summary>
        private bool _loginOnce = Configs.GetValue("loginOnce").ToBool();
        /// <summary>
        /// 验证码
        /// </summary>
        private string _adminCode = "Mock_session_verifycode";
        /// <summary>
        /// 登录token
        /// </summary>
        private string _adminToken = "Admin_Token";

        #endregion

        #region 封装HTTP对象
        #region Http上下文
        /// <summary>
        /// Http上下文
        /// </summary>
        HttpContext ContextHttp => HttpContext.Current;

        #endregion

        #region Response对象
        public HttpResponse Response => ContextHttp.Response;

        #endregion

        #region Request对象
        public HttpRequest Request => ContextHttp.Request;

        #endregion

        #region Session对象
        public HttpSessionState Session => ContextHttp.Session;

        #endregion
        #endregion


        #region 当前登录用户信息cookie或者session CurrentUser

        /// <summary>
        /// 当前登录用户信息cookie或者session
        /// </summary>
        public OperatorModel CurrentUser
        {
            get
            {
                var operatorModel = _loginProvider == "Cookie" 
                    ? DesEncrypt.Decrypt(WebHelper.GetCookie(_loginUserKey)?.ToString()).ToObject<OperatorModel>()
                    : DesEncrypt.Decrypt(WebHelper.GetSession(_loginUserKey)?.ToString()).ToObject<OperatorModel>();
                return operatorModel;
            }

            set
            {
                if (_loginProvider == "Cookie")
                {
                    WebHelper.WriteCookie(_loginUserKey, DesEncrypt.Encrypt(value.ToJson()), 60);
                    WebHelper.WriteCookie("moblog_mac", Md5Helper.Md5(Net.GetMacByNetworkInterface().ToJson(), 32));
                    WebHelper.WriteCookie("moblog_licence", Licence.GetLicence());
                }
                else
                {
                    WebHelper.WriteSession(_loginUserKey, DesEncrypt.Encrypt(value.ToJson()));
                }
                HttpRuntime.Cache[value.UserId.ToString()] = HttpContext.Current.Session.SessionID;
            }
        }
        #endregion

        #region 清空当前登录信息 RemoveCurrent
        /// <summary>
        /// 清空当前登录信息
        /// </summary>
        public void RemoveCurrent()
        {
            if (_loginProvider == "Cookie")
            {
                WebHelper.RemoveCookie(_loginUserKey.Trim());
            }
            else
            {
                WebHelper.RemoveSession(_loginUserKey.Trim());
            }
        }
        #endregion
        
        /// <summary>
        /// 当前登录的验证码
        /// </summary>
        public string CurrentCode
        {
            get => DesEncrypt.Decrypt(WebHelper.GetSession(_adminCode)?.ToString());
            set => WebHelper.WriteSession(_adminCode,DesEncrypt.Encrypt(value));
        }

        /// <summary>
        /// 当前唯一值token
        /// </summary>
        public string CurrentToken
        {
            get => DesEncrypt.Decrypt(WebHelper.GetSession(_adminToken)?.ToString());
            set => WebHelper.WriteSession(_adminToken, DesEncrypt.Encrypt(value));
        }

    }
}

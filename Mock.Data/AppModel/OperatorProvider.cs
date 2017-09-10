using Mock.Code;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mock.Data
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
        private string LoginUserKey = "mock_luo_loginuserkey_2017";
        /// <summary>
        /// 配置信息session或cookies
        /// </summary>
        private string LoginProvider = Configs.GetValue("LoginProvider");
        /// <summary>
        /// 单点登录标志
        /// </summary>
        private bool loginOnce = Configs.GetValue("loginOnce").ToBool();
        /// <summary>
        /// 保存当前用户权限信息
        /// </summary>
        private string Admin_Module_Permission = "Admin_Module_Permission";
        /// <summary>
        /// 验证码
        /// </summary>
        private string Admin_Code = "Admin_Code";
        /// <summary>
        /// 登录token
        /// </summary>
        private string Admin_Token = "Admin_Token";
    
        #endregion

        #region 当前登录用户信息cookie或者session CurrentUser
        /// <summary>
        /// 当前登录用户信息cookie或者session
        /// </summary>
        /// <param name="operatorModel"></param>
        public OperatorModel CurrentUser
        {
            get
            {
                OperatorModel operatorModel = new OperatorModel();
                if (LoginProvider == "Cookie")
                {
                    operatorModel = DESEncrypt.Decrypt(WebHelper.GetCookie(LoginUserKey)?.ToString()).ToObject<OperatorModel>();
                }
                else
                {
                    operatorModel = DESEncrypt.Decrypt(WebHelper.GetSession(LoginUserKey)?.ToString()).ToObject<OperatorModel>();
                }
                return operatorModel;
            }

            set
            {
                if (LoginProvider == "Cookie")
                {
                    WebHelper.WriteCookie(LoginUserKey, DESEncrypt.Encrypt(value.ToJson()), 60);
                    WebHelper.WriteCookie("moblog_mac", Md5.md5(Net.GetMacByNetworkInterface().ToJson(), 32));
                    WebHelper.WriteCookie("moblog_licence", Licence.GetLicence());
                }
                else
                {
                    WebHelper.WriteSession(LoginUserKey, DESEncrypt.Encrypt(value.ToJson()));
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
            if (LoginProvider == "Cookie")
            {
                WebHelper.RemoveCookie(LoginUserKey.Trim());
            }
            else
            {
                WebHelper.RemoveSession(LoginUserKey.Trim());
            }
        }
        #endregion

        #region 当前用户权限 ModulePermission
        /// <summary>
        /// 当前用户权限
        /// </summary>
        public List<AppModule> ModulePermission
        {
            get => DESEncrypt.Decrypt(WebHelper.GetSession(Admin_Module_Permission)?.ToString()).ToObject<List<AppModule>>();
            set => WebHelper.WriteSession(Admin_Module_Permission, DESEncrypt.Encrypt(value.ToJson()));
        } 
        #endregion
        /// <summary>
        /// 当前登录的验证码
        /// </summary>
        public string CurrentCode
        {
            get => DESEncrypt.Decrypt(WebHelper.GetSession(Admin_Code)?.ToString());
            set => WebHelper.WriteSession(Admin_Code,DESEncrypt.Encrypt(value));
        }
        /// <summary>
        /// 当前唯一值token
        /// </summary>
        public string CurrentToken
        {
            get => DESEncrypt.Decrypt(WebHelper.GetSession(Admin_Token)?.ToString());
            set => WebHelper.WriteSession(Admin_Token, DESEncrypt.Encrypt(value));
        }

    }
}

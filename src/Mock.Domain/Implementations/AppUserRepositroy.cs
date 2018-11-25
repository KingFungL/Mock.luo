using System;
using System.Linq;
using System.Linq.Expressions;
using Mock.Code;
using Mock.Code.Attribute;
using Mock.Code.Cache;
using Mock.Code.Configs;
using Mock.Code.Extend;
using Mock.Code.Helper;
using Mock.Code.Log;
using Mock.Code.Mail;
using Mock.Code.Net;
using Mock.Code.Security;
using Mock.Code.Validate;
using Mock.Code.Web;
using Mock.Data.AppModel;
using Mock.Data.ExtensionModel;
using Mock.Data.Extensions;
using Mock.Data.Models;
using Mock.Data.Repository;
using Mock.Domain.Interface;

namespace Mock.Domain.Implementations
{
    /// <summary>
    /// 仓储实现层 AppUserRepositroy
    /// </summary>]
    public class AppUserRepositroy : RepositoryBase<AppUser>, IAppUserRepository
    {
        #region 构造方法
        private readonly IAppModuleRepository _moduleService;
        private readonly IRedisHelper _iRedisHelper;
        private readonly IMailHelper _imailHelper;
        private readonly ILogMessageRepository _logService;
        public AppUserRepositroy(IAppModuleRepository moduleService, IRedisHelper iRedisHepler, IMailHelper imailHelper,ILogMessageRepository logService)
        {
            this._moduleService = moduleService;
            this._iRedisHelper = iRedisHepler;
            this._logService = logService;
            this._imailHelper = imailHelper;
        }
        #endregion

        #region 根据条件得到用户列表数据
        public DataGrid GetDataGrid(PageDto pag, string loginName, string email)
        {

            Expression<Func<AppUser, bool>> predicate = u => u.DeleteMark == false && (loginName == "" || u.LoginName.Contains(loginName))
            && (email == "" || u.Email.Contains(email));

            var dglist = this.Queryable(predicate).Where(pag).Select(u => new
            {
                u.LoginName,
                u.NickName,
                u.Email,
                u.Id,
                u.LoginCount,
                u.LastLoginTime,
                u.StatusCode,
                isBindQQ = u.AppUserAuths.Select(r => r.IdentityType == "QQ").Any(),
                UserRoleList = u.UserRoles.Select(r => r.AppRole.RoleName)
            }).ToList();
            return new DataGrid { Rows = dglist, Total = pag.Total };
        }
        #endregion

        #region  新增用户，编辑用户信息
        public void SubmitForm(AppUser userEntity, string roleIds)
        {

            if (userEntity.Id == 0)
            {
                userEntity.Create();
                string userPassword = "123qwe";//默认密码
                userEntity.UserSecretkey = Md5Helper.Md5(Utils.CreateNo(), 16).ToLower();
                userEntity.LoginPassword = Md5Helper.Md5(DesEncrypt.Encrypt(Md5Helper.Md5(userPassword, 32).ToLower(), userEntity.UserSecretkey).ToLower(), 32).ToLower();
                userEntity.LoginCount = 0;

                //新增时配置角色
                if (roleIds.IsNotNullOrEmpty())
                {
                    foreach (string id in roleIds.Split(','))
                    {
                        int.TryParse(id, out int result);
                        if (result == 0) continue;
                        AppUserRole userRoleEntity = new AppUserRole { RoleId = result };
                        userEntity.UserRoles.Add(userRoleEntity);
                    }
                }

                this.Insert(userEntity);

            }
            else
            {

                using (var db = new RepositoryBase().BeginTrans())
                {
                    userEntity.Modify(userEntity.Id);
                    string[] modifystrs = { "LoginName", "StatusCode", "Email", "NickName", "LastModifyUserId", "LastModifyTime" };
                    this.Update(userEntity, modifystrs);

                    db.Delete<AppUserRole>(u => u.UserId == userEntity.Id);

                    if (roleIds.IsNotNullOrEmpty())
                    {
                        foreach (string id in roleIds.Split(','))
                        {
                            int.TryParse(id, out int result);
                            if (result == 0) continue;
                            AppUserRole userRoleEntity = new AppUserRole { RoleId = result, UserId = (int)userEntity.Id };
                            db.Insert(userRoleEntity);
                        }
                    }
                    db.Commit();
                }
            }
        }
        #endregion

        #region  判断用户是否重复，用户的LoginName是否重复，Email是否重复
        public AjaxResult IsRepeat(AppUser userEntity)
        {
            Expression<Func<AppUser, bool>> predicate = u => u.DeleteMark == false;

            if (userEntity.Id == 0)
            {
                if (userEntity.LoginName.IsNullOrEmpty())
                {
                    return AjaxResult.Success("用户名为空");
                }
                var loginNameExpression = predicate.And(r => r.LoginName == userEntity.LoginName);
                if (this.Queryable(loginNameExpression).Any())
                {
                    return AjaxResult.Error("用户名已存在！");
                }
                else
                {
                    var emailExpression = predicate.And(r => r.Email == userEntity.Email);
                    if (this.Queryable(emailExpression).Any())
                    {
                        return AjaxResult.Error("此邮箱已存在！");
                    }
                }
            }
            else
            {
                if (userEntity.LoginName.IsNullOrEmpty())
                {
                    return AjaxResult.Success("用户名为空");
                }
                var loginNameExpression = predicate.And(r => r.LoginName == userEntity.LoginName && r.Id != userEntity.Id);
                if (this.Queryable(loginNameExpression).Any())
                {
                    return AjaxResult.Error("用户名已存在！");
                }
                else
                {
                    var emailExpression = predicate.And(r => userEntity.Email != "" && r.Email == userEntity.Email && r.Id != userEntity.Id);
                    if (this.Queryable(emailExpression).Any())
                    {
                        return AjaxResult.Error("此邮箱已存在！");
                    }
                }
            }
            return AjaxResult.Success("邮箱与用户名都可用！");
        }
        #endregion

        #region 验证登录
        public AjaxResult CheckLogin(string loginName, string pwd, bool rememberMe)
        {
            LogMessage logEntity = new LogMessage
            {
                CategoryId = 1,
                OperateType = EnumAttribute.GetDescription(DbLogType.Login),
                OperateAccount = loginName,
                Module = Configs.GetValue("SoftName")
            };
            AjaxResult ajaxResult;
            try
            {
                var userEntity = this.Queryable().FirstOrDefault(t => (t.LoginName == loginName || t.Email == loginName) && t.DeleteMark == false);

                if (userEntity != null)
                {
                    if (userEntity.UserSecretkey.IsNullOrEmpty())
                    {
                        throw new Exception("用户密钥丢失，请联系管理员！");
                    }
                    //登录成功
                    string dbPassword = Md5Helper.Md5(DesEncrypt.Encrypt(pwd.ToLower(), userEntity.UserSecretkey).ToLower(), 32).ToLower();
                    //登录成功
                    if (dbPassword == userEntity.LoginPassword)
                    {
                        string backUrl = "";
                        //根据登录实体，去缓存用户数据
                        this.SaveUserSession(userEntity);

                        backUrl = OperatorProvider.Provider.CurrentUser.IsSystem==true 
                            ? "/Home/Index" 
                            : "/App/Index";

                        //记住密码
                        if (rememberMe == true)
                        {

                        }

                        ajaxResult = AjaxResult.Success("登录成功!", backUrl);

                        DateTime now = DateTime.Now;
                        userEntity.LoginCount += 1;
                        userEntity.LastLoginTime = now;
                        userEntity.LastLogIp = Net.Ip;
                        userEntity.LastModifyTime = now;

                        logEntity.ExecuteResult = 1;
                        logEntity.ExecuteResultJson = "登录成功";

                        this.Update(userEntity, "LoginCount", "LastLoginTime", "LastLogIp", "LastModifyTime");

                    }
                    else
                    {
                        throw new Exception("密码不正确，请重新输入");
                    }
                }
                else
                {
                    throw new Exception("账户不存在，请重新输入");
                }
            }
            catch (Exception ex)
            {
                ajaxResult = AjaxResult.Error(ex.Message);

                logEntity.ExecuteResult = -1;
                logEntity.ExecuteResultJson = ex.Message;// new logformat().exceptionformat(logentity);
                //logEntity.ExceptionInfo = ex.Message ;

                _logService.LogError(logEntity, "登录日志");

            }

            return ajaxResult;

        }
        #endregion

        #region 重置密码

        public void ResetPassword(AppUser userEntity, string userPassword)
        {
            userEntity.UserSecretkey = Md5Helper.Md5(Utils.CreateNo(), 16).ToLower();
            userEntity.LoginPassword = Md5Helper.Md5(DesEncrypt.Encrypt(Md5Helper.Md5(userPassword, 32).ToLower(), userEntity.UserSecretkey).ToLower(), 32).ToLower();

            string[] modifstr = { "UserSecretkey", "LoginPassword", };

            this.Update(userEntity, modifstr);
        }
        #endregion

        #region 使用session暂存登录用户信息
        /// <summary>
        /// 使用session暂存登录用户信息
        /// </summary>
        /// <param name="userEntity"></param>
        public void SaveUserSession(AppUser userEntity)
        {
            OperatorProvider op = OperatorProvider.Provider;

            bool isSystem = this.IsSystem(userEntity.Id);

            //保存用户信息
            op.CurrentUser = new OperatorModel
            {
                UserId = userEntity.Id,
                IsSystem = isSystem,
                IsAdmin=userEntity.LoginName=="admin"?true:false,
                LoginName = userEntity.LoginName,
                LoginToken = Guid.NewGuid().ToString(),
                UserCode = "1234",
                LoginTime = DateTime.Now,
                NickName = userEntity.NickName,
                Avatar = userEntity.Avatar,
                Email = userEntity.Email,
                PersonalWebsite = userEntity.PersonalWebsite
            };
            //缓存存放单点登录信息
            ICache cache = CacheFactory.Cache();
            op.Session[userEntity.Id.ToString()] = userEntity.LoginName;//必须使用这个存储一下session，否则sessionid在每一次请求的时候，都会为变更
            cache.WriteCache<string>(userEntity.Id.ToString(), op.Session.SessionID, DateTime.UtcNow.AddMinutes(60));

            //登录权限分配,根据用户Id获取用户所拥有的权限，可以在登录之后的Home界面中统一获取。

            _iRedisHelper.KeyDeleteAsync(string.Format(ConstHelper.AppModule, "AuthorizeUrl_" + userEntity.Id));

        }
        #endregion

        #region  重置密码，发送验证码 SmsCode
        public AjaxResult SmsCode(string email)
        {
            AjaxResult amm;
            int limitcount = 10;
            int limitMinutes = 10;

            if (!Validate.IsEmail(email))
            {
                return AjaxResult.Error("邮箱格式不正确");
            }

            AppUser userEntity = this.Queryable(u => u.Email == email && u.DeleteMark == false).FirstOrDefault();

            if (userEntity == null)
            {
                amm = AjaxResult.Error("此邮箱尚未注册！");
            }
            else
            {
                string count = _iRedisHelper.StringGet<string>(email);
                //缓存十分钟，如果缓存中存在,且请求次数超过10次，则返回
                if (!string.IsNullOrEmpty(count) && Convert.ToInt32(count) >= limitcount)
                {
                    amm = AjaxResult.Error("没收到邮箱：请联系igeekfan@163.com");
                }
                else
                {
                    #region 发送邮箱，并写入缓存，更新登录信息表的token,date,code
                    int num = 0;
                    if (!string.IsNullOrEmpty(count))
                    {
                        num = Convert.ToInt32(count);
                    }
                    string countplus1 = num + 1 + "";
                    _iRedisHelper.StringSet<string>(email, countplus1, new TimeSpan(0, limitMinutes, 0));

                    ResetPwd resetpwdEntry = new ResetPwd
                    {
                        UserId = (int)userEntity.Id,
                        ModifyPwdToken = Utils.GuId(),
                        PwdCodeTme = DateTime.Now,
                        ModfiyPwdCode = Utils.RndNum(6),
                        LoginName = userEntity.LoginName,
                        NickName = userEntity.NickName
                    };

                    //将发送验证码的数据存入redis缓存中

                    _iRedisHelper.StringSet(email + "sendcodekey", resetpwdEntry, new TimeSpan(0, limitMinutes, 0));
                    //发送找回密码的邮件

                    string body = UiHelper.FormatEmail(resetpwdEntry, "PwdReSetTemplate");
                    _imailHelper.SendByThread(email, "[、天上有木月博客] 密码找回", body);

                    #endregion
                    //将修改密码的token返回给前端
                    amm = AjaxResult.Info("验证码已发送至你的邮箱！", resetpwdEntry.ModifyPwdToken, ResultType.Success.ToString());
                }
            }
            return amm;
        }
        #endregion

        #region 重置密码 ResetPwd
        public AjaxResult ResetPwd(string pwdtoken, string account, string newpwd, string emailcode)
        {
            AjaxResult amm;

            var userEntity = this.Queryable(u => u.Email == account).FirstOrDefault();

            if (userEntity != null)
            {
                //从缓存中取出存放的验证码的键（邮箱+"sendcodekey"）得到重置密码的对象
                ResetPwd rpwdEntry = _iRedisHelper.StringGet<ResetPwd>(account + "sendcodekey");

                if (rpwdEntry != null && rpwdEntry.ModifyPwdToken == pwdtoken)
                {
                    if (rpwdEntry.ModfiyPwdCode != emailcode)
                    {
                        amm = AjaxResult.Error("验证码不正确！");
                    }
                    else
                    {
                        string cacheaccount = _iRedisHelper.StringGet<string>(account + "modfiyPwdKey");
                        if (!string.IsNullOrEmpty(cacheaccount))
                        {
                            amm = AjaxResult.Error("10分钟内只可修改一次密码，请稍后再试!");
                        }
                        else
                        {  //重置密码成功之后将帐号写入cache中，10分钟内只可修改一次密码

                            ResetPassword(userEntity, newpwd);
                            amm = AjaxResult.Success("重置密码成功！");

                            _iRedisHelper.StringSet<string>(account + "modfiyPwdKey", account, new TimeSpan(0, 10, 0));
                        }
                    }
                }
                else
                {
                    amm = AjaxResult.Error("验证码过期了！");
                }
            }
            else
            {
                amm = AjaxResult.Error("此帐号尚未注册");
            }
            return amm;
        }
        #endregion

        #region 根据用户id判断用户是否是管理员
        public bool IsSystem(int? id)
        {
            return base.Db.Set<AppUserRole>().Where(r => r.UserId == id).Select(r => r.AppRole.RoleName).Any(r => r.Contains("管理员"));
        } 
        #endregion
    }
}

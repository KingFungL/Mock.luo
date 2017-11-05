using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock.Code;
using System.Linq.Expressions;
using System.Web;

namespace Mock.Domain
{
    /// <summary>
    /// 仓储实现层 AppUserRepositroy
    /// </summary>]
    public class AppUserRepositroy : RepositoryBase<AppUser>, IAppUserRepository
    {
        #region 构造方法
        private IAppModuleRepository _ModuleService;
        public AppUserRepositroy(IAppModuleRepository _ModuleService)
        {
            this._ModuleService = _ModuleService;
        }
        #endregion

        #region 根据条件得到用户列表数据
        public DataGrid GetDataGrid(Pagination pag, string LoginName, string Email)
        {

            Expression<Func<AppUser, bool>> predicate = u => u.DeleteMark == false && (LoginName == "" || u.LoginName.Contains(LoginName))
            && (Email == "" || u.Email.Contains(Email));

            var dglist = this.IQueryable(predicate).Where(pag).Select(u => new
            {
                u.LoginName,
                u.NickName,
                u.Email,
                u.Id,
                u.LoginCount,
                u.LastLoginTime,
                u.StatusCode,
                UserRoleList = u.UserRoles.Select(r => r.AppRole.RoleName)
            }).ToList();
            return new DataGrid { rows = dglist, total = pag.total };
        }
        #endregion

        #region  新增用户，编辑用户信息
        public void SubmitForm(AppUser userEntity, string roleIds)
        {

            if (userEntity.Id == 0)
            {
                userEntity.Create();
                string userPassword = "1234";//默认密码
                userEntity.UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                userEntity.LoginPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userPassword, 32).ToLower(), userEntity.UserSecretkey).ToLower(), 32).ToLower();
                userEntity.LoginCount = 0;

                //新增时配置角色
                if (roleIds.IsNotNullOrEmpty())
                {
                    foreach (string id in roleIds.Split(','))
                    {
                        int.TryParse(id, out int result);
                        if (result == 0) continue;
                        UserRole userRoleEntity = new UserRole { RoleId = result };
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

                    db.Delete<UserRole>(u => u.UserId == userEntity.Id);

                    if (roleIds.IsNotNullOrEmpty())
                    {
                        foreach (string id in roleIds.Split(','))
                        {
                            int.TryParse(id, out int result);
                            if (result == 0) continue;
                            UserRole userRoleEntity = new UserRole { RoleId = result, UserId = (int)userEntity.Id };
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
                var loginNameExpression = predicate.And(r => r.LoginName == userEntity.LoginName);
                if (this.IQueryable(loginNameExpression).Count() > 0)
                {
                    return AjaxResult.Error("用户名已存在！");
                }
                else
                {
                    var emailExpression = predicate.And(r => r.Email == userEntity.Email);
                    if (this.IQueryable(emailExpression).Count() > 0)
                    {
                        return AjaxResult.Error("此邮箱已存在！");
                    }
                }
            }
            else
            {
                var loginNameExpression = predicate.And(r => r.LoginName == userEntity.LoginName && r.Id != userEntity.Id);
                if (this.IQueryable(loginNameExpression).Count() > 0)
                {
                    return AjaxResult.Error("用户名已存在！");
                }
                else
                {
                    var emailExpression = predicate.And(r => userEntity.Email != "" && r.Email == userEntity.Email && r.Id != userEntity.Id);
                    if (this.IQueryable(emailExpression).Count() > 0)
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

            AjaxResult ajaxResult;
            try
            {
                AppUser userEntity = new AppUser { };

                userEntity = this.IQueryable().Where(t => (t.LoginName == loginName || t.Email == loginName) && t.DeleteMark == false).FirstOrDefault();

                if (userEntity != null)
                {
                    //登录成功
                    string dbPassword = Md5.md5(DESEncrypt.Encrypt(pwd.ToLower(), userEntity.UserSecretkey).ToLower(), 32).ToLower();
                    //登录成功
                    if (dbPassword == userEntity.LoginPassword)
                    {
                        //根据登录实体，去缓存用户数据
                        this.SaveUserSession(userEntity);

                        //记住密码
                        if (rememberMe == true)
                        {

                        }
                        ajaxResult = AjaxResult.Success("登录成功!");
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
                Log log = LogFactory.GetLogger("登录日志记录");
                log.Error(ex.Message);
            }


            return ajaxResult;

        }
        #endregion

        #region 重置密码

        public void ResetPassword(int keyValue, string userPassword)
        {
            AppUser userEntity = new AppUser();
            userEntity.Id = keyValue;
            userEntity.UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
            userEntity.LoginPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userPassword, 32).ToLower(), userEntity.UserSecretkey).ToLower(), 32).ToLower();

            string[] modifstr = { "UserSecretkey", "LoginPassword", };

            this.Update(userEntity, modifstr);
        }
        #endregion

        /// <summary>
        /// 使用session暂存登录用户信息
        /// </summary>
        /// <param name="userEntity"></param>
        public void SaveUserSession(AppUser userEntity)
        {
            OperatorProvider op = OperatorProvider.Provider;

            //保存用户信息
            op.CurrentUser = new OperatorModel
            {
                UserId = userEntity.Id,
                IsSystem = userEntity.LoginName == "admin" ? true : false,
                LoginName = userEntity.LoginName,
                LoginToken = Guid.NewGuid().ToString(),
                UserCode = "1234",
                LoginTime = DateTime.Now,
                NickName = userEntity.NickName,
                Avatar = userEntity.Avatar
            };
            //缓存存放单点登录信息
            ICache cache = CacheFactory.Cache();
            op.Session[userEntity.Id.ToString()] = userEntity.LoginName;//必须使用这个存储一下session，否则sessionid在每一次请求的时候，都会为变更
            cache.WriteCache<string>(userEntity.Id.ToString(), op.Session.SessionID, DateTime.UtcNow.AddMinutes(60));

            //登录权限分配,根据用户Id获取用户所拥有的权限，可以在登录之后的Home界面中统一获取。
            //op.ModulePermission = _ModuleService.GetUserModules(userEntity.Id);
            op.ModulePermission = null;
        }

    }
}

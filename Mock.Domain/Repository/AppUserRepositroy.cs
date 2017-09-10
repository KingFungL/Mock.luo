using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock.Code;
using System.Linq.Expressions;

namespace Mock.Domain
{
    /// <summary>
    /// 仓储实现层 ArticleRepositroy
    /// </summary>]
    public class AppUserRepositroy : RepositoryBase<AppUser>, IAppUserRepository
    {
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

        public void SubmitForm(AppUser userEntity, string roleIds)
        {

            if (userEntity.Id == 0)
            {
                userEntity.Create();
                string userPassword = "1234";//默认密码
                userEntity.UserSecretkey = Md5.md5(Utils.GuId(), 16).ToLower();
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
                    var emailExpression = predicate.And(r => r.Email == userEntity.Email && r.Id != userEntity.Id);
                    if (this.IQueryable(emailExpression).Count() > 0)
                    {
                        return AjaxResult.Error("此邮箱已存在！");
                    }
                }
            }
            return AjaxResult.Success("邮箱与用户名都可用！");
        }

        public AjaxResult CheckLogin(string loginName, string pwd)
        {

            AjaxResult ajaxResult;
            try
            {
                OperatorProvider op = OperatorProvider.Provider;
                //if (!oc.CurrentUserVcode.Equals(code.ToLower()))
                //{
                //    throw new Exception("验证码错误，请重新输入!");
                //}
                AppUser userEntity = this.IQueryable().Where(t => (t.LoginName == loginName||t.Email==loginName) && t.DeleteMark == false).FirstOrDefault();

                if (userEntity != null)
                {
                    //登录成功
                    string dbPassword = Md5.md5(DESEncrypt.Encrypt(pwd.ToLower(), userEntity.UserSecretkey).ToLower(), 32).ToLower();
                    //登录成功
                    if (dbPassword == userEntity.LoginPassword)
                    {
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
            }
            return ajaxResult;

        }
    }
}

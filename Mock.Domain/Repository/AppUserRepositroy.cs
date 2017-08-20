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
                u.LastLoginTime
            }).ToList();

            return new DataGrid { rows = dglist, total = pag.total };

        }

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
                    string[] modifystrs = { "LoginName", "Phone", "Email", "Birthday", "PersonalWebsite", "NickName", "PersonSignature", "HeadHref", "Sex", "LastModifyUserId", "LastModifyTime" };
                    this.Update(userEntity, modifystrs);

                    db.Delete<UserRole>(u => u.UserId == userEntity.Id);

                    if (roleIds.IsNotNullOrEmpty())
                    {
                        foreach (string id in roleIds.Split(','))
                        {
                            int.TryParse(id, out int result);
                            if (result == 0) continue;
                            UserRole userRoleEntity = new UserRole { RoleId = result ,UserId=(int)userEntity.Id}; 
                            db.Insert(userRoleEntity);
                        }
                    }
                    db.Commit();
                }
            }
        }


        public AjaxResult IsRepeat(AppUser userEntity)
        {

            Expression<Func<AppUser, bool>> predicate = u => u.DeleteMark==false;

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
                    if(this.IQueryable(emailExpression).Count()>0)
                    {
                        return AjaxResult.Error("此邮箱已存在！");
                    }
                }
            }
            else
            {
                var loginNameExpression = predicate.And(r => r.LoginName == userEntity.LoginName&&r.Id!=userEntity.Id);
                if (this.IQueryable(loginNameExpression).Count() > 0)
                {
                    return AjaxResult.Error("用户名已存在！");
                }
                else
                {
                    var emailExpression = predicate.And(r => r.Email == userEntity.Email&&r.Id!=userEntity.Id);
                    if (this.IQueryable(emailExpression).Count() > 0)
                    {
                        return AjaxResult.Error("此邮箱已存在！");
                    }
                }
            }
            return AjaxResult.Success("邮箱与用户名都可用！");
        }


    }
}

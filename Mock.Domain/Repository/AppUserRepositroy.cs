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

        public void Edit(AppUser userEntity)
        {

            if (userEntity.Id == 0)
            {
                userEntity.Create();
                this.Insert(userEntity);
            }
            else
            {
                userEntity.Modify(userEntity.Id);
                string[] modifystrs = { "LoginName", "Phone", "Email", "Birthday", "PersonalWebsite", "NickName", "PersonSignature", "HeadHref", "Sex", "LastModifyUserId", "LastModifyTime" };
                this.Update(userEntity, modifystrs);
            }
        }


    }
}

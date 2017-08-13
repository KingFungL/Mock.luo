using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock.Code;

namespace Mock.Domain
{
    /// <summary>
    /// 仓储实现层 ArticleRepositroy
    /// </summary>]
    public class AppUserRepositroy : RepositoryBase<AppUser>, IAppUserRepository
    {
        public DataGrid GetDataGrid(Pagination pag)
        {

            var dglist = this.IQueryable(u => u.DeleteMark == false).Where(pag).Select(u => new
            {
                u.LoginName,
                u.NickName,
                u.Email,
                u.Id
            }).ToList();

            return new DataGrid { rows = dglist, total = pag.total };

        }

        public dynamic GetFormById(int Id)
        {
            var d = this.IQueryable(u => u.Id == Id).Select(u => new
            {
            }).FirstOrDefault();
            return d;
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
                string[] modifystrs = { "LoginName", "Phone", "Email", "Birthday", "PersonalWebsite", "NickName", "PersonSignature ", "HeadHref", "Sex", "LastModifyUserId", "LastModifyTime" };
                this.Update(userEntity, modifystrs);
            }
        }


    }
}

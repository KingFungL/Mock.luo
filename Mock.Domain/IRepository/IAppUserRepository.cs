using Mock.Code;
using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Domain
{
    public interface IAppUserRepository : IRepositoryBase<AppUser>
    {
        DataGrid GetDataGrid(Pagination pag,string LoginName,string Email);
         void SubmitForm(AppUser userEntity,string roleIds);
        /// <summary>
        /// 判断用户是否重复，用户的LoginName是否重复，Email是否重复
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        AjaxResult IsRepeat(AppUser userEntity);
    }
}

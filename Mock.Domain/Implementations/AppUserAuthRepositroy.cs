using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock.Code;
using System.Linq.Expressions;
using Mock.Domain.Interface;
using EntityFramework.Extensions;

namespace Mock.Domain
{

    public class AppUserAuthRepositroy : RepositoryBase<AppUserAuth>, IAppUserAuthRepository
    {

        public Task DeleteAsync(int Id)
        {
            AppUserAuth user = new AppUserAuth
            {
                Id = Id
            };
            return Db.Set<AppUserAuth>().Where(u => u.Id == Id).DeleteAsync();
        }
    }
}

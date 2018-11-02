using System.Linq;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using Mock.Data.Models;
using Mock.Data.Repository;
using Mock.Domain.Interface;

namespace Mock.Domain.Implementations
{

    public class AppUserAuthRepositroy : RepositoryBase<AppUserAuth>, IAppUserAuthRepository
    {

        public Task DeleteAsync(int id)
        {
            AppUserAuth user = new AppUserAuth
            {
                Id = id
            };
            return Db.Set<AppUserAuth>().Where(u => u.Id == id).DeleteAsync();
        }
    }
}

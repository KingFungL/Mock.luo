using System.Collections.Generic;
using Mock.Code.Web;
using Mock.Data.Models;
using Mock.Data.Repository;

namespace Mock.Domain.Interface
{
    public interface IUserRoleRepository : IRepositoryBase<AppUserRole>
    {
        /// <summary>
        /// 保存角色对应的用户
        /// </summary>
        /// <param name="entities">用户角色关联List实体</param>
        /// <param name="roleId">角色id</param>
        /// <returns></returns>
        AjaxResult SaveMembers(List<AppUserRole> entities, int roleId);
        /// <summary>
        /// 根据角色id得到分配的用户数据
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        dynamic GetAllotUserGrid(int roleId);
    }
}

using Mock.Code;
using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Domain
{
    public interface IUserRoleRepository : IRepositoryBase<UserRole>
    {
        /// <summary>
        /// 保存角色对应的用户
        /// </summary>
        /// <param name="entities">用户角色关联List实体</param>
        /// <param name="roleId">角色id</param>
        /// <returns></returns>
        AjaxResult SaveMembers(List<UserRole> entities, int roleId);
        /// <summary>
        /// 根据角色id得到分配的用户数据
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        dynamic GetAllotUserGrid(int roleId);
    }
}

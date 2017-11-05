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
    public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    {
        #region 根据角色id得到分配的用户数据
        public dynamic GetAllotUserGrid(int roleId)
        {
            var usersActiveList = this.Db.Set<AppUser>().AsNoTracking().Where(u => u.DeleteMark == false).Select(u => new
            {
                u.Id,
                u.NickName,
                u.LoginName,
                u.Gender,
                u.Avatar,
                u.LoginCount,
                u.LastLoginTime,
                u.LastLogIp,
                IsActive = u.UserRoles.FirstOrDefault() != null && u.UserRoles.Where(r => r.RoleId == roleId && r.UserId == u.Id).Count() > 0
            }).OrderByDescending(u => u.IsActive == true).ToList();
            return usersActiveList;
        }
        #endregion

        #region 保存角色对应的用户
        public AjaxResult SaveMembers(List<UserRole> entities, int roleId)
        {
            AjaxResult msg;
            int iret = 0;
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<UserRole>(u => u.RoleId == roleId);
                db.Insert(entities);
                iret = db.Commit();
            }
            if (iret > 0)
            {
                msg = AjaxResult.Success("角色成员配置成功！");
            }
            else
            {
                msg = AjaxResult.Error("出错了！");
            }
            return msg;
        }
        #endregion
    }
}

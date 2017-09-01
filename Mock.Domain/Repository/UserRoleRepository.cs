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
        public dynamic GetAllotUserGrid(int roleId)
        {
            var usersActiveList = this.db.Set<AppUser>().AsNoTracking().Where(u=>u.DeleteMark==false).Select(u => new
            {
                u.Id,
                u.NickName,
                u.LoginName,
                u.Sex,
                u.HeadHref,
                u.LoginCount,
                u.LastLoginTime,
                u.LastLogIp,
                IsActive = u.UserRoles.FirstOrDefault() != null && u.UserRoles.Where(r => r.RoleId == roleId && r.UserId == u.Id).Count() > 0
            }).OrderByDescending(u => u.IsActive == true).ToList();
            return usersActiveList;
        }

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
    }
}

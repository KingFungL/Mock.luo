/*
   * 创建者：天上有木月
   * 创建时间：2018/10/30 14:34:25
   * 邮箱：igeekfan@foxmail.com
   * 文件功能描述： 
   * 
   * 修改人： 
   * 时间：
   * 修改说明：
   */

using Mock.Code;
using Mock.Code.Security;
using Mock.Data.Models;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Mock.Data.Migrations.SeedData
{
    public class CreateRoleAndUser
    {
        private readonly MockDbContext _dbContext;
        public CreateRoleAndUser(MockDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void Excute()
        {
            if (!_dbContext.AppRole.Any())
            {
                _dbContext.AppRole.Add(new AppRole()
                {
                    RoleName = "超级管理员",
                    IsEnableMark = true
                });

            }

            if (!_dbContext.AppUser.Any())
            {
                string userSecretkey = Md5Helper.Md5(Utils.CreateNo(), 16).ToLower();

                string loginPassword = Md5Helper.Md5(DesEncrypt.Encrypt(Md5Helper.Md5("123qwe", 32).ToLower(), userSecretkey).ToLower(), 32).ToLower();

                _dbContext.AppUser.AddOrUpdate(new AppUser()
                {
                    LoginName = "admin",
                    LoginPassword = loginPassword,
                    UserSecretkey = userSecretkey
                });

            }

            _dbContext.SaveChanges();
            if (!_dbContext.UserRole.Any())
            {
                List<AppUser> users = _dbContext.AppUser.ToList();
                users?.ForEach(r =>
                {
                    _dbContext.UserRole.Add(new AppUserRole()
                    {
                        UserId = r.Id,
                        RoleId = 1
                    });
                });

            }

        }

    }
}

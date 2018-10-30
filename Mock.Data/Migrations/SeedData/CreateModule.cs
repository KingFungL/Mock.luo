/*
   * 创建者：天上有木月
   * 创建时间：2018/10/30 14:01:10
   * 邮箱：igeekfan@foxmail.com
   * 文件功能描述： 
   * 
   * 修改人： 
   * 时间：
   * 修改说明：
   */

using Mock.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mock.Data.Migrations.SeedData
{
    public class CreateModule
    {
        private readonly MockDbContext _dbContext;
        public CreateModule(MockDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void Excute()
        {
            string iframe = "iframe";
            string expand = "expand";
            string menu = "Menu";
            string pmenu = "PMenu";
            string permission = "Permission";

            if (!_dbContext.AppModule.Any())
            {

                _dbContext.AppModule.AddRange(new List<AppModule>()
                {
                    new AppModule(){PId = 0,Name = "系统管理",LinkUrl = "/Plat/AppModule/Index",Target = expand,TypeCode = pmenu,Icon = "fa fa-home",},
                    new AppModule(){PId = 1,Name = "用户管理",LinkUrl = "/Plat/AppUser/Index",Target = iframe,TypeCode = menu,Icon = "fa fa-user-o",},
                    new AppModule(){PId = 1,Name = "角色管理",LinkUrl = "/Plat/AppRole/Index",Target = iframe,TypeCode = menu,Icon = "fa fa-user-circle",},
                    new AppModule(){PId = 1,Name = "系统功能",LinkUrl = "/Plat/AppModule/Index",Target = iframe,TypeCode = menu,Icon = "fa fa-anchor",},
                    new AppModule(){PId = 1,Name = "测试管理",LinkUrl = "/Home/TestView",Target = iframe,TypeCode = menu,Icon = "fa fa-street-view",},
                    new AppModule(){PId = 1,Name = "系统日志",LinkUrl = "/Plat/LogInfo/Index",Target = iframe,TypeCode = menu,Icon = "fa fa-home",},

                    new AppModule(){PId = 0,Name = "博客管理",Target = expand,TypeCode = pmenu,Icon = "fa fa-cloud-download",},
                    new AppModule(){PId = 1,Name = "字典管理",LinkUrl = "/Plat/ItemsDetail/Index",Target = iframe,TypeCode = menu,Icon = "fa fa-key",},
                    new AppModule(){PId = 0,Name = "文章管理",LinkUrl = "/Plat/Article/Index",Target = iframe,TypeCode = menu,Icon = "fa fa-file-text-o",},
                    new AppModule(){PId = 0,Name = "留言审核",LinkUrl = "/Plat/GuestBook/Index",Target = iframe,TypeCode = menu,Icon = "fa fa-pencil",},
                    new AppModule(){PId = 0,Name = "评论审核",LinkUrl = "/Plat/Review/Index",Target = iframe,TypeCode = menu,Icon = "fa fa-product-hunt",},


                    new AppModule(){PId = 2,Name = "编辑用户页面",LinkUrl = "/Plat/AppUser/Form",Target = iframe,TypeCode = permission},
                    new AppModule(){PId = 2,Name = "新增用户页面",LinkUrl = "/Plat/AppUser/Form",Target = iframe,TypeCode = permission},
                    new AppModule(){PId = 2,Name = "刷新用户信息",LinkUrl = "/Plat/AppUser/GetDataGrid",Target = iframe,TypeCode = permission},
                    new AppModule(){PId = 2,Name = "重置密码",LinkUrl = "/Plat/AppUser/ResetPassword",Target = iframe,TypeCode = permission},
                    new AppModule(){PId = 2,Name = "删除用户",LinkUrl = "/Plat/AppUser/Delete",Target = iframe,TypeCode = permission},

                    new AppModule(){PId = 3,Name = "角色授权权限",LinkUrl = "/Plat/AppRole/SaveAuthorize",Target = iframe,TypeCode = permission},
                    new AppModule(){PId = 3,Name = "角色授权界面",LinkUrl = "/Plat/AppRole/AllotAuthorize",Target = iframe,TypeCode = permission},
                    new AppModule(){PId = 3,Name = "角色成员权限",LinkUrl = "/Plat/AppRole/SaveMembers",Target = iframe,TypeCode = permission},
                    new AppModule(){PId = 3,Name = "角色成员界面",LinkUrl = "/Plat/AppRole/AllotUser",Target = iframe,TypeCode = permission},
                    new AppModule(){PId = 3,Name = "保存角色权限",LinkUrl = "/Plat/AppRole/Edit",Target = iframe,TypeCode = permission},
                    new AppModule(){PId = 3,Name = "删除角色权限",LinkUrl = "/Plat/AppRole/Delete",Target = iframe,TypeCode = permission},
                    new AppModule(){PId = 3,Name = "编辑角色界面",LinkUrl = "/Plat/AppRole/Form",Target = iframe,TypeCode = permission},
                    new AppModule(){PId = 3,Name = "新增角色界面",LinkUrl = "/Plat/AppRole/Form",Target = iframe,TypeCode = permission},
                    new AppModule(){PId = 3,Name = "刷新角色列表",LinkUrl = "/Plat/AppRole/GetDataGrid",Target = iframe,TypeCode = permission},

                });
            }

            _dbContext.SaveChanges();

            if (!_dbContext.RoleMenu.Any())
            {
                foreach (var appModule in _dbContext.AppModule.ToList())
                {
                    _dbContext.RoleMenu.Add(new AppRoleModule()
                    {
                        ModuleId = appModule.Id,
                        RoleId = 1
                    });
                }
            }
        }
    }
}

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
        /// <summary>
        /// 根据条件得到用户列表数据
        /// </summary>
        /// <param name="pag">分页条件</param>
        /// <param name="LoginName">登录名</param>
        /// <param name="Email">邮箱</param>
        /// <returns></returns>
        DataGrid GetDataGrid(Pagination pag, string LoginName, string Email);
        /// <summary>
        /// 新增用户，编辑用户信息
        /// </summary>
        /// <param name="userEntity">用户实体</param>
        /// <param name="roleIds">角色id，以逗号分隔</param>
         void SubmitForm(AppUser userEntity,string roleIds);
        /// <summary>
        /// 判断用户是否重复，用户的LoginName是否重复，Email是否重复
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        AjaxResult IsRepeat(AppUser userEntity);
        /// <summary>
        /// 验证登录
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pwd">密码</param>
        /// <returns>成功与否</returns>
        AjaxResult CheckLogin(string loginName, string pwd,bool rememberMe);
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="userPassword">用户密码</param>
        void ResetPassword(int keyValue, string userPassword);
        /// <summary>
        /// 根据登录的实体，通过session保存用户信息
        /// </summary>
        /// <param name="userEntity"></param>
        void SaveUserSession(AppUser userEntity);
    }
}

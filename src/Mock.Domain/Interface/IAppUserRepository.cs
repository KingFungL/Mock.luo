using Mock.Code.Web;
using Mock.Data.AppModel;
using Mock.Data.Models;
using Mock.Data.Repository;

namespace Mock.Domain.Interface
{
    public interface IAppUserRepository : IRepositoryBase<AppUser>
    {
        /// <summary>
        /// 根据条件得到用户列表数据
        /// </summary>
        /// <param name="pag">分页条件</param>
        /// <param name="loginName">登录名</param>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        DataGrid GetDataGrid(PageDto pag, string loginName, string email);
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
        /// <param name="userEntity">用户实体</param>
        /// <param name="userPassword">用户密码</param>
        void ResetPassword(AppUser userEntity, string userPassword);
        /// <summary>
        /// 根据登录的实体，通过session保存用户信息
        /// </summary>
        /// <param name="userEntity"></param>
        void SaveUserSession(AppUser userEntity);
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        AjaxResult SmsCode(string email);
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="pwdtoken">密码token，判断唯一性</param>
        /// <param name="account">邮件</param>
        /// <param name="newpwd">新密码</param>
        /// <param name="emailcode">邮件验证码</param>
        /// <returns></returns>
        AjaxResult ResetPwd(string pwdtoken, string account, string newpwd, string emailcode);


        /// <summary>
        /// 根据用户id判断用户是否昌管理员
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        bool IsSystem(int? id);
    }
}

using System.Collections.Generic;
using Mock.Data.AppModel;
using Mock.Data.Models;
using Mock.Data.Repository;

namespace Mock.Domain.Interface
{
    public interface IAppRoleRepository : IRepositoryBase<AppRole>
    {
        /// <summary>
        /// 获取角色下拉框
        /// </summary>
        /// <returns></returns>
        dynamic GetRoleJson();
        /// <summary>
        /// 不分页的角色列表数据
        /// </summary>
        /// <returns>DataGrid实体</returns>

        DataGrid GetDataGrid(string search);
        /// <summary>
        /// 保存角色下的权限信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="roleModules"></param>
        void SaveAuthorize(int roleId, List<AppRoleModule> roleModules);
    }
}

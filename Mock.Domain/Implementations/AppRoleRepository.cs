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

    public class AppRoleRepository : RepositoryBase<AppRole>, IAppRoleRepository
    {

        #region 角色下拉框
        public dynamic GetRoleJson()
        {
            var entities = this.IQueryable(u => u.DeleteMark == false && u.IsEnableMark == true).OrderBy(u => u.SortCode).Select(u => new
            {
                id = u.Id,
                text = u.RoleName
            }).ToList();

            return entities;
        }
        #endregion

        #region 不分页的角色列表数据 DataGrid实体
        public DataGrid GetDataGrid(string search)
        {
            Expression<Func<AppRole, bool>> predicate = u => u.DeleteMark == false
            && (search == "" || u.RoleName.Contains(search))
            && (search == "" || u.Remark.Contains(search));
            var entities = this.IQueryable(predicate).OrderBy(u => u.SortCode).ThenByDescending(r => r.Id).Select(u => new
            {
                u.Id,
                u.RoleName,
                u.SortCode,
                u.Remark,
                u.IsEnableMark
            }).ToList();
            return new DataGrid { rows = entities, total = entities.Count() };
        } 
        #endregion

        #region 保存角色配置权限信息
        public void SaveAuthorize(int roleId, List<RoleModule> roleModules)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<RoleModule>(u => u.RoleId == roleId);
                if (roleModules.Any())
                {
                    db.Insert(roleModules);
                }
                db.Commit();
            }
        } 
        #endregion
    }
}

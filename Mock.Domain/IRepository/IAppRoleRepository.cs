﻿using Mock.Code;
using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Domain
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
        /// <returns></returns>

        DataGrid GetDataGrid(string search);
        

    }
}
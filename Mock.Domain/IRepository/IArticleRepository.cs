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
    public interface IArticleRepository : IRepositoryBase<Article>
    {
        dynamic GetFormById(int Id);
        DataGrid GetDataGrid(Pagination pag);
       void Edit(Article entity);

        //得到最新的count篇文章
        dynamic GetRecentArticle(int count);

        DataGrid GetIndexGird(Pagination pag);
    }
}

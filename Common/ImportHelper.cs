using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ImportHelper
    {
        #region 保存List集合进数据库 InsertListIntoSql
        /// <summary>
        /// 保存List集合进数据库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbConnection">数据库连接</param>
        /// <param name="tableName">表名</param>
        /// <param name="list">list对象</param>
        public static void InsertListIntoSql<T>(DbConnection dbConnection, string tableName, IList<T> list)
        {
            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }
            //执行批量插入的方法
            BulkHelper.BulkInsert((SqlConnection)dbConnection, tableName, list);

            if (dbConnection.State != ConnectionState.Closed)
            {
                dbConnection.Close();
            }

        }
        #endregion
    }
}

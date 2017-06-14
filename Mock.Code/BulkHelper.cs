using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class BulkHelper
    {
        #region BulkInsert 批量导入
        /// <summary>
        /// Function:批量导入
        /// Author:zhb
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn">联接字符串</param>
        /// <param name="tableName">表名</param>
        /// <param name="list">数据列表</param>
        public static void BulkInsert<T>(SqlConnection conn, string tableName, IList<T> list)
        {
            using (var bulkCopy = new SqlBulkCopy(conn))
            {
                bulkCopy.BatchSize = list.Count;
                bulkCopy.DestinationTableName = tableName;

                var table = new DataTable();
                var props = TypeDescriptor.GetProperties(typeof(T))
                    .Cast<PropertyDescriptor>()
                    .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
                    .Where(u => u.Name != "IsExcelVaildateOK").ToArray();

                foreach (var propertyInfo in props)
                {

                    bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                    table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);

                }

                var values = new object[props.Length];
                foreach (var item in list)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }

                    table.Rows.Add(values);
                }
                bulkCopy.WriteToServer(table);
            }
        }
        #endregion

        #region MultiUpdateData 批量更新数据
        /// <summary>
        /// Funciton:批量更新List<T>到数据库中-如果是更新List<T>首先调用list.ListToDataTable();方法，将List<T>转成DataTable
        /// Author:luozQ
        /// Date:2017-5-24
        /// </summary>
        /// <param name="data">数据源</param>
        /// <param name="Columns">需要更新的列名</param>
        /// <param name="tableName">表名</param>
        /// <param name="connectionString">联接字符串</param>
        /// <returns></returns>
        public static bool MultiUpdateData(DataTable data, string Columns, string tableName, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string SQLString = string.Format("select {0} from {1}", Columns, tableName);
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataAdapter myDataAdapter = new SqlDataAdapter();
                        myDataAdapter.SelectCommand = new SqlCommand(SQLString, connection);
                        SqlCommandBuilder custCB = new SqlCommandBuilder(myDataAdapter);
                        custCB.ConflictOption = ConflictOption.OverwriteChanges;
                        custCB.SetAllValues = true;
                        foreach (DataRow dr in data.Rows)
                        {
                            if (dr.RowState == DataRowState.Unchanged)
                                dr.SetModified();
                        }
                        myDataAdapter.Update(data);
                        data.AcceptChanges();
                        myDataAdapter.Dispose();
                        return true;
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        connection.Close();
                        throw E;
                    }
                }
            }
        }

        #endregion

    }
}

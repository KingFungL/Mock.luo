using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Extensions
{
    /// <summary>  
    /// sql函数的扩展类  
    /// </summary>  
    public static class SqlFunctionExtensions
    {
        #region 功能方法  
        /// <summary>  
        /// 在linq to entity中使用SqlServer.NEWID函数  
        /// </summary>  
        [System.Data.Entity.DbFunction("SqlServer", "NEWID")]
        public static Guid NewId()
        {
            return Guid.NewGuid();
        }
        #endregion

        #region 扩展方法  
        /// <summary>  
        /// 随机排序扩展方法  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="source"></param>  
        /// <returns></returns>  
        public static IQueryable<T> OrderByNewId<T>(this IEnumerable<T> source)
        {
            return source.AsQueryable().OrderBy(d => NewId());
        }
        #endregion

    }
}

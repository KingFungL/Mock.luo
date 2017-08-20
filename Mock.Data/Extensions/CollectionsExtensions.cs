using Mock.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Mock.Data
{
    /// <summary>
    /// 存放查询的扩展方法
    /// </summary>
    public static class CollectionsExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> source, Pagination pagination) where T : class, new()
        {
            return source.Where(u => true, pagination);
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, Pagination pagination) where T : class, new()
        {
            MethodCallExpression resultExp = null;
            var tempData = source.Where(predicate);

            List<string> _sort = pagination.sort == null ? new List<string>() { } : pagination.sort.Split(',').ToList();
            List<bool> _isAsc = pagination.order == null ? new List<bool> { } : pagination.order.Split(',').Select(v => { return v.ToUpper() == "ASC"; }).ToList();

            if (_sort.Count() == 0 || _isAsc.Count() == 0)
            {
                ArgumentNullException e = new ArgumentNullException("sort");
                throw new Exception(string.Format("参数 {0} 为空引发异常。", "sort"), e);
            }

            if (_sort.Count() != _isAsc.Count())
            {
                throw new Exception("参数不正确！");
            }
            int i = 0;
            foreach (string item in _sort)
            {

                var parameter = Expression.Parameter(typeof(T), "t");
                var property = typeof(T).GetProperty(item);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var sortByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), _isAsc[i++] ? "OrderBy" : "OrderByDescending", new Type[] { typeof(T), property.PropertyType }, tempData.Expression, Expression.Quote(sortByExp));
            }
            tempData = tempData.Provider.CreateQuery<T>(resultExp);
            pagination.total = tempData.Count();
            tempData = tempData.Skip<T>(pagination.offset).Take<T>(pagination.limit).AsQueryable();
            return tempData;
        }

    }
}

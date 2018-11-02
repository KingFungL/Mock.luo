using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mock.Code.Web;

namespace Mock.Data.Extensions
{
    /// <summary>
    /// 存放查询的扩展方法
    /// </summary>
    public static class CollectionsExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> source, PageDto pagination) where T : class, new()
        {
            return source.Where(u => true, pagination);
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, PageDto pagination) where T : class, new()
        {
            MethodCallExpression resultExp = null;
            var tempData = source.Where(predicate);

            List<string> sort = pagination.Sort == null ? new List<string>() { } : pagination.Sort.Split(',').ToList();
            List<bool> isAsc = pagination.Order == null ? new List<bool> { } : pagination.Order.Split(',').Select(v => { return v.ToUpper() == "ASC"; }).ToList();

            if (!sort.Any() || !isAsc.Any())
            {
                ArgumentNullException e = new ArgumentNullException("sort");
                throw new Exception($"参数 {"sort"} 为空引发异常。", e);
            }

            if (sort.Count() != isAsc.Count())
            {
                throw new Exception("参数不正确！");
            }
            int i = 0;
            foreach (string item in sort)
            {

                var parameter = Expression.Parameter(typeof(T), "t");
                var property = typeof(T).GetProperty(item);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var sortByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc[i++] ? "OrderBy" : "OrderByDescending", new Type[] { typeof(T), property.PropertyType }, tempData.Expression, Expression.Quote(sortByExp));
            }
            tempData = tempData.Provider.CreateQuery<T>(resultExp);
            pagination.Total = tempData.Count();
            tempData = tempData.Skip<T>(pagination.Offset).Take<T>(pagination.Limit).AsQueryable();
            return tempData;
        }

    }
}

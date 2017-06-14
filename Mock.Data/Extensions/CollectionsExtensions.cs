using Mock.Code;
using System;
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
            bool isAsc = pagination.order == null ? true : pagination.order.ToLower() == "asc" ? true : false;
            string[] _order = pagination.sort.Split(',');
            MethodCallExpression resultExp = null;
            var tempData = source.Where(predicate);
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(T), "t");
                var property = typeof(T).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(T), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<T>(resultExp);
            pagination.total = tempData.Count();
            tempData = tempData.Skip<T>(pagination.rows * (pagination.page - 1)).Take<T>(pagination.rows).AsQueryable();
            return tempData;
        }
    }
}

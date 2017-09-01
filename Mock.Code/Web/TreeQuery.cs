
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mock.Code
{
    public static class TreeQuery
    {
        /// <summary>
        /// 通过深度优先搜索找到树形结构下的子节点，不过这个泛型的keyValue要是string类型,parentId也要string类型，不然没法转换成labam表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        /// <param name="condition"></param>
        /// <param name="keyValue"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<T> TreeWhere<T>(this List<T> entityList, Predicate<T> condition, string keyValue = "id", string parentId = "parentId") where T : class
        {
            List<T> locateList = entityList.FindAll(condition);
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "t");
            List<T> treeList = new List<T>();
            foreach (T entity in locateList)
            {
                treeList.Add(entity);
                string pId = entity.GetType().GetProperty(parentId).GetValue(entity, null).ToString();
                while (true)
                {
                    if (string.IsNullOrEmpty(pId) || pId == "0")
                    {
                        break;
                    }
                    Predicate<T> upLambda = (System.Linq.Expressions.Expression.Equal(parameter.Property(keyValue), System.Linq.Expressions.Expression.Constant(pId))).ToLambda<Predicate<T>>(parameter).Compile();
                    T upRecord = entityList.Find(upLambda);
                    if (upRecord != null)
                    {
                        treeList.Add(upRecord);
                        pId = upRecord.GetType().GetProperty(parentId).GetValue(upRecord, null).ToString();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return treeList.Distinct().ToList();
        }

        /// <summary>
        /// 深度优先搜索到符合条的子节点
        /// </summary>
        /// <param name="entityList"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static List<TreeSelectModel> TreeWhere(this List<TreeSelectModel> entityList, Predicate<TreeSelectModel> condition) 
        {
            List<TreeSelectModel> locateList = entityList.FindAll(condition);
            List<TreeSelectModel> treeList = new List<TreeSelectModel>();
            foreach (TreeSelectModel item in locateList)
            {
                treeList.Add(item);
                string pId = item.parentId;
                while (true)
                {
                    if (pId==null||pId=="0")
                    {
                        break;
                    }
                    Predicate<TreeSelectModel> upLambda = u => u.id == pId;
                    TreeSelectModel upRecord = entityList.Find(upLambda);
                    if (upRecord != null)
                    {
                        treeList.Add(upRecord);
                        pId = upRecord.parentId;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return treeList.Distinct().ToList();
        }
    }
}

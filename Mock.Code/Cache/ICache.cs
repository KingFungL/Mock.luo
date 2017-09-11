
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Code
{    /// <summary>
     /// 缓存接口类
     /// author:luozQ
     /// date:2017-6-20
     /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 根据键值得到缓存数据
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="cacheKey">key值</param>
        /// <returns></returns>
        T GetCache<T>(string cacheKey) where T : class;
        /// <summary>
        /// 插入一个键值，不过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        void Insert<T>(string chcheKey, T value) where T : class;
        /// <summary>
        /// 根据键值写入缓存数据，绝对过期时间为10分钟
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">要插入到缓存中的对象</param>
        /// <param name="cacheKey">用于该对象的缓存键</param>
        void WriteCache<T>(string cacheKey, T value) where T : class;
        /// <summary>
        /// 根据键值写入缓存数据，绝对过期时间为DateTime.UtcNow
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="value">值</param>
        /// <param name="cacheKey">键</param>
        /// <param name="expireTime">过期日期</param>
        void WriteCache<T>(string cacheKey, T value, DateTime expireTime) where T : class;
        /// <summary>
        /// 从应用程序的 System.Web.Caching.Cache 对象移除指定项
        /// </summary>
        /// <param name="cacheKey">要移除的缓存项的 System.String 标识符。</param>
        void RemoveCache(string cacheKey);
        /// <summary>
        /// 从应用程序的 System.Web.Caching.Cache 对象移除所有缓存
        /// </summary>
        void RemoveCache();
    }
}

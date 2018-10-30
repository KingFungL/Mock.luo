using System;
using System.Collections;
using System.Web;

namespace Mock.Code.Cache
{
    public class Cache : ICache
    {
        private static System.Web.Caching.Cache _cache = HttpRuntime.Cache;
        /// <summary>
        /// 根据键值得到缓存数据
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="cacheKey">key值</param>
        /// <returns></returns>
        public T GetCache<T>(string cacheKey) where T : class
        {
            if (_cache[cacheKey] != null)
            {
                return (T)_cache[cacheKey];
            }
            return default(T);
        }
        /// <summary>
        /// 插入一个键值，不过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        public void Insert<T>(string cacheKey, T value) where T : class
        {
            _cache.Insert(cacheKey, value);
        }
        /// <summary>
        /// 根据键值写入缓存数据，绝对过期时间为10分钟
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">要插入到缓存中的对象</param>
        /// <param name="cacheKey">用于该对象的缓存键</param>
        public void WriteCache<T>( string cacheKey,T value) where T : class
        {
            _cache.Insert(cacheKey, value, null, DateTime.UtcNow.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);
        }
        /// <summary>
        /// 根据键值写入缓存数据，绝对过期时间为DateTime.UtcNow
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="value">值</param>
        /// <param name="cacheKey">键</param>
        /// <param name="expireTime">过期日期</param>
        public void WriteCache<T>(string cacheKey, T value,  DateTime expireTime) where T : class
        {
            _cache.Insert(cacheKey, value, null, expireTime, System.Web.Caching.Cache.NoSlidingExpiration);
        }
        /// <summary>
        /// 从应用程序的 System.Web.Caching.Cache 对象移除指定项
        /// </summary>
        /// <param name="cacheKey">要移除的缓存项的 System.String 标识符。</param>
        public void RemoveCache(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }
        /// <summary>
        /// 从应用程序的 System.Web.Caching.Cache 对象移除所有缓存
        /// </summary>
        public void RemoveCache()
        {
            IDictionaryEnumerator cacheEnum = _cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                _cache.Remove(cacheEnum.Key.ToString());
            }
        }
    }
}

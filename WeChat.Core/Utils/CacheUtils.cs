using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Utils
{
    /// <summary>
    /// 缓存类
    /// </summary>
    public static class CacheUtils
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expire"></param>
        public static void Add(string key, object value, DateTime expire)
        {
            System.Web.HttpContext.Current.Cache.Add(key, value, null, expire,
                System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return System.Web.HttpContext.Current.Cache.Get(key);
        }


    }
}

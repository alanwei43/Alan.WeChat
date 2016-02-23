using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Example.Library
{
    /// <summary>
    /// 缓存服务
    /// </summary>
    public static class MemCache
    {
        private static IDictionary<string, CacheObj<object>> _dict;

        static MemCache()
        {
            _dict = new Dictionary<string, CacheObj<object>>();
        }

        public static void Add<T>(string key, DateTime expire, T value)
        {
            if (expire == default(DateTime)) return;

            var cache = new CacheObj<object> { Expire = expire, Value = value };
            if (_dict.ContainsKey(key))
            {
                _dict[key] = cache;
            }

            _dict.Add(key, cache);
        }

        public static T Get<T>(string key)
            where T : class
        {
            if (_dict.ContainsKey(key))
            {
                var cache = _dict[key];
                if (cache == null || !cache.IsValid()) return default(T);
                return cache.Value as T;
            }
            return default(T);
        }

        public class CacheObj<T>
        {
            public DateTime Expire { get; set; }
            public T Value { get; set; }

            public bool IsValid()
            {
                return DateTime.Now < Expire;
            }
        }
    }
}
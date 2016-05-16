using Alan.Log.LogContainerImplement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alan.Log.Core;
using Alan.Utils.ExtensionMethods;

namespace WeChat.Core.Cache
{
    /// <summary>
    /// 基于内存实现的ICache
    /// </summary>
    public class MemoryCache : ICache
    {
        private IDictionary<string, CacheObj<object>> _dict;

        public MemoryCache()
        {
            _dict = new Dictionary<string, CacheObj<object>>();
        }


        public T Get<T>(string key)
            where T : class
        {
            var obj = this.Get(key);
            if (obj == null) return default(T);
            return obj as T;
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

        public void Add(string key, object value, DateTime expire)
        {

            if (expire == default(DateTime)) return;

            var cache = new CacheObj<object> { Expire = expire, Value = value };
            _dict[key] = cache;
        }

        public object Get(string key)
        {
            if (_dict.ContainsKey(key))
            {
                var cache = _dict[key];
                if (cache == null || !cache.IsValid()) return null;
                return cache.Value;
            }
            return null;
        }
    }
}

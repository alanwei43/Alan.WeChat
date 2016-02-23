using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Cache
{
    /// <summary>
    /// Cache辅助类
    /// </summary>
    public class CacheUtils
    {
        /// <summary>
        /// 当前注入的ICache对象
        /// </summary>
        public static ICache Current { get; private set; }
        static CacheUtils()
        {
            Current = new MemoryCache();
        }
        /// <summary>
        /// 注入ICache对象
        /// </summary>
        /// <param name="cache"></param>
        public static void Inject(ICache cache)
        {
            Current = cache;
        }
    }
}

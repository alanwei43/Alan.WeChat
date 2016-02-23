using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Cache
{
    public class CacheUtils
    {
        public static ICache Current { get; private set; }
        static CacheUtils()
        {
            Current = new MemoryCache();
        }
        public static void Inject(ICache cache)
        {
            Current = cache;
        }
    }
}

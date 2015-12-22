using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Log
{
    public static class LogUtils
    {
        public static ILog Current { get; private set; }

        static LogUtils()
        {
            Inject(new TraceLog());
        }

        public static void Inject(ILog log)
        {
            Current = log;
        }

        public static void Write(this ILog log, string id, DateTime date, string category, string note)
        {
            log.Write(id, date, category, null, null, note);
        }

        public static void Write(this ILog log, DateTime date, string category, string note)
        {

            log.Write(null, date, category, null, null, note);
        }

        public static void Write(this ILog log, string id, string category, string note)
        {
            log.Write(id, DateTime.Now, category, null, null, note);
        }

        public static void Write(this ILog log, string category, string note)
        {
            log.Write(null, DateTime.Now, category, null, null, note);
        }

        public static void Write(this ILog log, string note)
        {
            log.Write(null, DateTime.Now, null, null, null, note);
        }

        public static void WriteWithOutId(this ILog log, string category, string note)
        {
            log.Write((System.Web.HttpContext.Current.Items["X-Request-Id"] ?? "").ToString(), category, note);
        }
        public static void WriteWithOutId(this ILog log, string note)
        {
            log.Write((System.Web.HttpContext.Current.Items["X-Request-Id"] ?? "").ToString(), note);
        }
    }
}

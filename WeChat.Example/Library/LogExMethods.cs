using Alan.Log.Core;

namespace WeChat.Example.Library
{
    public static class LogExMethods
    {

        public static void LogWithId(this ILogContainer log, string category, string note)
        {
            var id = (System.Web.HttpContext.Current.Items["X-Request-Id"] ?? "").ToString();
            log.Log(id: id, level: "debug", category: category, note: note);
        }
        public static void LogWithId(this ILogContainer log, string note)
        {
            var id = (System.Web.HttpContext.Current.Items["X-Request-Id"] ?? "").ToString();
            log.Log(id: id, level: "debug", category: null, note: note);
        }
    }
}

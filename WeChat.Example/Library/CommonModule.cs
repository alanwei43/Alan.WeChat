using System;
using System.Web;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Log;

namespace WeChat.Example.Library
{
    public class CommonModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
            context.PreSendRequestContent += Context_PreSendRequestContent;
            context.Error += Context_Error;
        }

        private void Context_PreSendRequestContent(object sender, EventArgs e)
        {
            LogUtils.Current.WriteWithOutId(category: "/Request/End", note: String.Format("{0}",DateTime.Now));
        }

        private void Context_Error(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            try
            {
                var lastEx = app.Server.GetLastError();
                if (lastEx != null)
                    LogUtils.Current.Write(
                        id: (System.Web.HttpContext.Current.Items["X-Request-Id"] ?? "").ToString(),
                        category: "exception",
                        note: new
                        {
                            Method = app.Request.HttpMethod,
                            Url = app.Request.RawUrl,
                            Message = lastEx.Message,
                            Stack = lastEx.StackTrace,
                            Source = lastEx.Source
                        }.ExToJson());
                else
                    LogUtils.Current.Write(
                        id: (System.Web.HttpContext.Current.Items["X-Request-Id"] ?? "").ToString(),
                        category: "exception",
                        note: String.Format("Exception: {0} \t {1} \t {2}",
                        DateTime.Now,
                        app.Request.HttpMethod,
                        app.Request.RawUrl));
            }
            catch { }
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            if (app == null) return;

            System.Web.HttpContext.Current.Items["X-Request-Id"] = Guid.NewGuid().ToString();

            LogUtils.Current.WriteWithOutId(
                category: "/Request/Start",
                note: String.Format("{0} \n\r {1} \n\r {2}", DateTime.Now, app.Request.HttpMethod, app.Request.RawUrl));

        }

        public void Dispose()
        {
        }
    }
}
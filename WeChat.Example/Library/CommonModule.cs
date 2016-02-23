using System;
using System.Web;
using Alan.Log.LogContainerImplement;
using Alan.Utils.ExtensionMethods;

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
            LogUtils.Current.LogWithId(category: "/Request/End", note: String.Format("{0}", DateTime.Now));
        }

        private void Context_Error(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            try
            {
                var lastEx = app.Server.GetLastError();

                LogUtils.Current.Log(
                    level: "error",
                    category: "exception",
                    note: new
                    {
                        Method = app.Request.HttpMethod,
                        Url = app.Request.RawUrl,
                        Message = lastEx.Message,
                        Stack = lastEx.StackTrace,
                        Source = lastEx.Source
                    }.ExToJson());

            }
            catch (Exception ex)
            {
                var appErroLog = app.Server.MapPath("~/App_Data/ApplicationError.log");
                if (!System.IO.File.Exists(appErroLog))
                {
                    using (var fs = System.IO.File.Create(appErroLog)) { }
                }
                System.IO.File.WriteAllText(appErroLog, String.Format("", ex.Message, ex.StackTrace));
            }
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            if (app == null) return;

            System.Web.HttpContext.Current.Items["X-Request-Id"] = Guid.NewGuid().ToString();

            LogUtils.Current.LogWithId(category: "/Request/Start", note: String.Format("{0} \n\r {1} \n\r {2} \n\r {3}", DateTime.Now, app.Request.UserHostAddress, app.Request.HttpMethod, app.Request.RawUrl));

        }

        public void Dispose()
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Alan.Log.LogContainerImplement;
using Alan.Log.Core;

namespace Alan.WeChat.CleverTangYuan
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var logFilePath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/log.txt");
            Alan.Log.LogContainerImplement.LogUtils.Current.InjectLogModule(new Alan.Log.ILogImplement.LogAutoSeperateFilesByDate(logFilePath));

            Library.WeChat.Register.Run();
        }



        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            LogUtils.Current.LogError(message: ex.Message, category: "/application/error");
            //Response.Redirect("unexpectederror.htm");
        }
    }
}
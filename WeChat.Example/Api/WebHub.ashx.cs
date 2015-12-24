using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Api;
using WeChat.Core.Api.ContensManage;
using WeChat.Core.Api.MenuManage;
using WeChat.Core.Log;
using WeChat.Core.Messages;
using WeChat.Core.Messages.Middlewares;
using WeChat.Example.Library;

namespace WeChat.Example.Api
{
    /// <summary>
    /// Summary description for WebHub
    /// </summary>
    public class WebHub : HttpTaskAsyncHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var req = context.Request;
            var rep = context.Response;
            var svr = context.Server;

            if (req.HttpMethod.ToUpper() == "GET")
            {
                rep.Write(req["echostr"]);
                return;
            }

            var responseText = Middleware.Execute(req).GetResponse();
            LogUtils.Current.WriteWithOutId(category: "/Message/Response", note: responseText);
            rep.Write(responseText);
        }

        public override async Task ProcessRequestAsync(HttpContext context)
        {
            var t = await DownloadMedia.DownloadAsync();
            System.IO.File.WriteAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("~/think.json"), t.FileData);
            context.Response.Write(Encoding.UTF8.GetString(t.FileData));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}
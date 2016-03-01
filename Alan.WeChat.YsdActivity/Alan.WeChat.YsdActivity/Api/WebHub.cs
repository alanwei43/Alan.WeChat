using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using WeChat.Core.Messages.Middlewares;
using System.Text;

namespace WeChat.YsdActivity.Api
{
    public class WebHub : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var req = context.Request;
            var rep = context.Response;
            var svr = context.Server;


            if (req.HttpMethod.ToUpper() == "GET")
            {
                rep.ContentEncoding = Encoding.UTF8;
                var echo = req["echostr"];
                rep.Write(echo);
                return;
            }

            var responseText = Middleware.Execute(req).GetResponse();
            rep.Write(responseText);
        }

        public bool IsReusable { get; }
    }
}
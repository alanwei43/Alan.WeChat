using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Api.UserGroupManage;
using WeChat.Core.Messages.Middlewares;
using WeChat.Core.Log;
using System.Text;
using WeChat.Core.Api;

namespace WeChat.Example.Api
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

                var user = "oLntxs13Y5LXfAeck1SJHRTVtd1Y";
                var remarkRep = RemarkUser.Remark(user, "remark test");
                rep.Write(remarkRep.ExToJson());
                return;
            }

            var responseText = Middleware.Execute(req).GetResponse();
            rep.Write(responseText);
        }

        public bool IsReusable { get; }
    }
}
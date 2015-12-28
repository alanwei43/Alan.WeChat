using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Api.UserGroupManage;
using WeChat.Core.Messages.Middlewares;

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
                //var createRep = CreateGroup.Create("hello");
                var queryRep = QueryGroups.Query();
                //rep.Write(createRep.ExToJson());
                rep.Write(queryRep.ExToJson());

                rep.Write(req["echostr"]);
                return;
            }

            var responseText = Middleware.Execute(req).GetResponse();
            rep.Write(responseText);
        }

        public bool IsReusable { get; }
    }
}
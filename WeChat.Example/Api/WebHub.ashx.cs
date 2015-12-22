﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChat.Core.Log;
using WeChat.Core.Messages;
using WeChat.Example.Library;

namespace WeChat.Example.Api
{
    /// <summary>
    /// Summary description for WebHub
    /// </summary>
    public class WebHub : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var req = context.Request;
            var rep = context.Response;

            if (req.HttpMethod.ToUpper() == "GET")
            {
                rep.Write(MyConfig.Current.SqlConnection);
                rep.Write(req["echostr"]);
                return;
            }

            var responseText = Middleware.Execute(req).GetResponse();
            LogUtils.Current.WriteWithOutId(category: "/Message/Response", note: responseText);
            rep.Write(responseText);
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
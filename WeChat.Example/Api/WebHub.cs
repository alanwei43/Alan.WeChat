using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using WeChat.Core.Messages.Middlewares;
using Alan.Log.LogContainerImplement;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using WeChat.Example.Library;
using Alan.Log.Core;
using Alan.Log.ILogImplement;

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
                return;
            }

            var apiReq = WebRequest.Create("http://apis.baidu.com/apistore/weatherservice/cityname?cityname=天津");
            apiReq.Headers.Add("apikey", MyConfig.Current.BaiduApiKey);
            using (var reader = new StreamReader(apiReq.GetResponse().GetResponseStream(), Encoding.UTF8))
            {
                var apiRep = reader.ReadToEnd();
                Encoding encode = new UnicodeEncoding();
                encode.GetString(Encoding.UTF8.GetBytes(apiRep));
            }

            var responseText = Middleware.Execute(req).GetResponse();
            rep.Write(responseText);
        }

        public bool IsReusable { get; }
    }
}
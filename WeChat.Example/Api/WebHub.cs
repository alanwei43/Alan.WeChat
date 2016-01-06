using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Api.UserGroupManage;
using WeChat.Core.Messages.Middlewares;
using WeChat.Core.Log;
using System.Text;
using WeChat.Core.Api;
using WeChat.Core.Api.ContensManage;
using WeChat.Core.Utils;
using WeChat.Core.EncryptDecrypt;

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

                //var mediaId = "sAmTzdZsPN9atbFbzjKJNtGRDGI16nWXodN2H2vAMpnnYVNt-sdREePHWmAIZ6Qy";
                //var mediaResponse = DownloadTempMedia.Download(mediaId);
                //System.IO.File.WriteAllBytes(@"D:\temp.jpg", mediaResponse.FileData);

                rep.ContentEncoding = Encoding.UTF8;
                rep.AddHeader("X-WeChat-AppId", WeChat.Core.Utils.Configurations.Current.AppId);
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
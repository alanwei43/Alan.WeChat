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
using WeChat.Core.Api.ContentsManage;
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
               var response =  AddNews.Add(new Core.Api.ContentsManage.Models.AddNewsModel
                {
                    articles = new List<Core.Api.ContentsManage.Models.NewsItemModel>
                    {
                        new Core.Api.ContentsManage.Models.NewsItemModel
                        {
                             author = "Alan",
                              content = "hello world.",
                               digest="this is disgest",
                               title = "this is title",
                               thumb_media_id="http://mmbiz.qpic.cn/mmbiz/jMKe1dXjj6OK96xcH0icoeOXBG4DoumdGgkVLVx8lCXvdSqyalG7MQFKZrxgSXgeiaicLF8sjAcQDQU8xHMG5TKMA/0",
                               show_cover_pic="0",
                               content_source_url = "http://www.bing.com"
                        }
                    }
                });

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
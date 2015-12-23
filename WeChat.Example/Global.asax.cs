using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Api;
using WeChat.Core.Log;
using WeChat.Core.Messages;
using WeChat.Core.Messages.Middlewares;
using WeChat.Core.Messages.Normal;
using WeChat.Core.Utils;
using WeChat.Example.Library;

namespace WeChat.Example
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            LogUtils.Inject(new DbLog());


            #region Middleware Inject

            Middleware.InjectGlobalPreFilter(middleware =>
            {
                //记录请求数据
                LogUtils.Current.WriteWithOutId(category: "/Message/Request/Data", note: middleware.Input.Request);
            });

            Middleware.ImageFilters.Inject((req, middleware) =>
            {
                LogUtils.Current.WriteWithOutId(category: "/Message/Image/Input", note: middleware.Input.Request);

                var repModel = new NewsResponse()
                {
                    FromUserName = req.ToUserName,
                    ToUserName = req.FromUserName,
                    Articles = new List<NewsResponse.ArticleItem>() {
                          new NewsResponse.ArticleItem()
                          {
                              Description = "Hello world.",
                              PicUrl = "https://www.baidu.com/img/bd_logo1.png",
                              Title = "余盆财富",
                              Url = "http://fishlove.yupen.cn/"
                          },
                          new NewsResponse.ArticleItem()
                          {
                              Description = "Hello world.",
                              PicUrl = "https://www.baidu.com/img/bd_logo1.png",
                              Title = "余盆财富1",
                              Url = "http://fishlove.yupen.cn/custom/index.html"
                          }
                     }
                };

                middleware.SetResponseModel(repModel);
            });

            Middleware.ClickFilters.Inject((click, middleware) =>
            {
                var user = WeChatUserInfo.Get(click.FromUserName);
                var textResponse = new TextResponse()
                {
                    Content = String.Format("Welcome {0}. you have clicked {1}.", user.NickName, click.EventKey),
                    FromUserName = click.ToUserName,
                    ToUserName = click.FromUserName,
                    MsgType = Configurations.Current.MessageType.Text
                };

                middleware.SetResponseModel(textResponse);
            });

            Middleware.TextFilters.Inject((textRequest, middleware) =>
            {
                var repModel = new TextResponse()
                {
                    FromUserName = textRequest.ToUserName,
                    ToUserName = textRequest.FromUserName,
                    Content = textRequest.Content + " - inject text filter",
                    MsgType = textRequest.MsgType
                };

                middleware.SetResponseModel(repModel);
            });

            #endregion

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
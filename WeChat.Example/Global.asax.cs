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
            Middleware.Inject(parameter =>
            {
                if (parameter.Input.RequestBaseModel.MsgType == Configurations.Current.MessageType.Text)
                {
                    var textRequest = parameter.Input.GetRequestModel<TextRequest>();
                    parameter.Output.ResponseModel = new TextResponse()
                    {
                        FromUserName = textRequest.ToUserName,
                        ToUserName = textRequest.FromUserName,
                        Content = textRequest.Content + " - demo",
                        MsgType = textRequest.MsgType
                    };
                    parameter.Output.Response = parameter.Output.ResponseModel.ToXml();
                }
                return parameter;
            });

            Middleware.Inject(parameter =>
            {
                if (parameter.Input.RequestBaseModel.MsgType == Configurations.Current.MessageType.Image)
                {
                    LogUtils.Current.WriteWithOutId(category: "/Message/Image/Input", note: parameter.Input.Request);
                    var imageRequest = parameter.Input.GetRequestModel<ImageRequest>();
                    LogUtils.Current.WriteWithOutId(category: "/Message/Image/Request", note: imageRequest.ExToJson());

                    var repModel = new NewsResponse()
                    {
                        FromUserName = parameter.Input.RequestBaseModel.ToUserName,
                        ToUserName = parameter.Input.RequestBaseModel.FromUserName,
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
                    var xml = repModel.ToXml();

                    LogUtils.Current.WriteWithOutId(category: "/Message/Image/Response", note: xml);

                    parameter.Output.ResponseModel = repModel;
                    parameter.Output.Response = repModel.ToXml();
                }

                if (parameter.Input.RequestBaseModel.MsgType == Configurations.Current.MessageType.Event)
                {
                    var eb = parameter.Input.GetRequestModel<EventBase>();
                    LogUtils.Current.WriteWithOutId(category: "/Message/Request/Event", note: eb.ExToJson());

                    var repModel = new NewsResponse()
                    {
                        FromUserName = parameter.Input.RequestBaseModel.ToUserName,
                        ToUserName = parameter.Input.RequestBaseModel.FromUserName,
                        Articles = new List<NewsResponse.ArticleItem>() {
                          new NewsResponse.ArticleItem()
                          {
                              Description = "Hello world.",
                              PicUrl = "http://yupen.cn/images/logo.jpg",
                              Title = "余盆财富",
                              Url = "http://www.yupen.cn/"
                          },
                          new NewsResponse.ArticleItem()
                          {
                              Description = "Hello world.",
                              PicUrl = "http://yupen.cn/images/logo.jpg",
                              Title = "余盆财富1",
                              Url = "http://www.yupen.cn/custom/index.html"
                          }
                     }
                    };
                    parameter.Output.ResponseModel = repModel;
                    parameter.Output.Response = repModel.ToXml();
                }

                return parameter;
            });

            Middleware.Inject(parameter =>
            {
                if (parameter.Input.RequestBaseModel.MsgType == Configurations.Current.MessageType.Event)
                {
                    var eventModel = parameter.Input.GetRequestModel<EventBase>();
                    if (eventModel.Event == "subscribe")

                    {
                        var user = WeChatUserInfo.Get(eventModel.FromUserName);
                        var textResponse = new TextResponse()
                        {
                            Content = String.Format("Welcome {0}.", user.NickName),
                            FromUserName = eventModel.ToUserName,
                            ToUserName = eventModel.FromUserName,
                            MsgType = Configurations.Current.MessageType.Text
                        };
                        parameter.Output.ResponseModel = textResponse;
                        parameter.Output.Response = textResponse.ToXml();

                    }
                }
                return parameter;
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
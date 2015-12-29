using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Api;
using WeChat.Core.Api.MenuManage;
using WeChat.Core.Log;
using WeChat.Core.Messages;
using WeChat.Core.Messages.Middlewares;
using WeChat.Core.Messages.Normal;
using WeChat.Core.Utils;
using WeChat.Example.Library;
using WeChat.Core.Messages.Events;

namespace WeChat.Example
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {



            WeChat.Core.Utils.FluentConfig.Get()
                .Inject(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Config.json")) //出入JSON文件的形式传入配置信息
                .Inject(new DbLog()) //注入日志模块, 不是必需的
                .Inject(middleware =>
                {
                    //记录请求数据
                    LogUtils.Current.WriteWithOutId(category: "/Message/Request/Data", note: middleware.Input.Request);
                })
                .InjectEnd(middleware =>
                {
                    //记录输出数据
                    LogUtils.Current.WriteWithOutId(category: "/Message/Response/Data", note: middleware.GetResponse());
                })

                //使用Inject方法注入文本消息过滤器
                //只有文本消息的内容是 "摄影大赛" 的时候才执行这个过滤器
                .InjectTxt(req => req.Content == "摄影大赛", req => new NewsResponse()
                {
                    Articles = new List<NewsResponse.ArticleItem>
                    {
                        new NewsResponse.ArticleItem
                        {
                            Description = "双鱼之恋 - 摄影大赛",
                            PicUrl = "http://fishlove.yupen.cn/Custom/images/home-slogan.png",
                            Title = "双鱼之恋 - 摄影大赛",
                            Url = "http://fishlove.yupen.cn/Custom/index.html?openid=" + req.FromUserName
                        }
                    }
                }
                )
                //使用InjectTxt方法注入文本消息过滤器
                //只有文本消息的内容是 "我的信息" 的时候才执行这个过滤器
                .InjectTxt(req => req.Content == "我的信息", req =>
                {
                    //获取微信用户信息
                    var user = WeChat.Core.Api.WeChatUserInfo.Get(req.FromUserName);
                    return new TextResponse()
                    {
                        Content = String.Format("你的名字是 {0}.", user.NickName),
                        MsgType = WeChat.Core.Utils.Configurations.Current.MessageType.Text
                    };
                })
                //使用InjectTxt方法注入文本消息过滤器
                //只有文本消息的内容是 "现在时间" 的时候才执行这个过滤器
                .InjectTxt(req => req.Content == "现在时间", req => new TextResponse
                {
                    Content = DateTime.Now.ToString("服务器时间 yyyy-MM-dd HH:mm:ss"),
                    MsgType = WeChat.Core.Utils.Configurations.Current.MessageType.Text
                })
                //当用户向微信公众号发送图片消息的响应
                .InjectImg(req => true, req =>
                {

                    var items = HttpUtils.Get(req.PicUrl, null, null).DownloadFile();
                    System.IO.File.WriteAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("~/" + items.Item1), items.Item2);
                    return new TextResponse
                    {
                        Content = "download file ",
                        MsgType = Configurations.Current.MessageType.Text
                    };
                })
                .InjectClick(req => req.EventKey == "photographer", req => new NewsResponse
                {
                    Articles = new List<NewsResponse.ArticleItem>
                    {
                        new NewsResponse.ArticleItem
                        {
                            Description = "双鱼之恋 - 摄影大赛",
                            PicUrl = "http://fishlove.yupen.cn/Custom/images/home-slogan.png",
                            Title = "双鱼之恋 - 摄影大赛",
                            Url = "http://fishlove.yupen.cn/Custom/index.html?openid=" + req.FromUserName
                        }
                    }
                })
                .InjectLoc(req => true, (req )=> new TextResponse
                {
                    Content = String.Format("你目前所在地是: {0}. ", req.Label),
                    MsgType = WeChat.Core.Utils.Configurations.Current.MessageType.Text
                })
                .InjectEvent(req => req.Event == Configurations.Current.EventType.PickSysPhoto, req => new TextResponse
                {
                    Content = "you select picksysphoto",
                    MsgType = Configurations.Current.MessageType.Text
                })
                .InjectTxt((req, middleware) => !middleware.SetedResponse, (req, middleware)=> new TextResponse
                {
                    Content = "你发送的信息是: " + req.Content + ".",
                    MsgType = Configurations.Current.MessageType.Text
                });

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
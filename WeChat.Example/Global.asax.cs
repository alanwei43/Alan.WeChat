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
            WeChat.Core.Api.MenuManage.CreateMenus.Create(new Dictionary<string, object>
            {
                {
                    "button", new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            {"type", "click"},
                            {"name","菜单1" },
                            {"key","menu1" }
                        },
                        new Dictionary<string, object>
                        {
                            {"name", "子菜单"},
                            {"sub_button", new List<Dictionary<string, object>>
                            {
                                new Dictionary<string, object>
                                {
                                    {"name","子1" },
                                    {"type","view" },
                                    {"key","submenu1" }
                                },
                                new Dictionary<string, object>
                                {
                                    {"name","子2" },
                                    {"type","view" },
                                    {"key","submenu2" }
                                }
                            } }
                        }
                    }
                }
            });


            WeChat.Core.Utils.FluentConfig.Get()
                //.Inject("token", "aes key", "app id", "app secret")   //已参数的形式传入配置信息
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
                //使用Inject方法注入单击菜单过滤器
                .Inject((ClickMenuRequest click, MiddlewareParameter middleware) =>
                {
                    if (click.EventKey == "help")
                    {
                        var txtRep = new TextResponse()
                        {
                            Content = "帮助信息",
                            MsgType = Configurations.Current.MessageType.Text
                        };
                        middleware.SetResponseModel(txtRep);
                    }
                })
                //使用Inject方法注入文本消息过滤器
                //只有文本消息的内容是 "摄影大赛" 的时候才执行这个过滤器
                .Inject((TextRequest textRequest, MiddlewareParameter middleware) =>
                {
                    if (textRequest.Content == "摄影大赛")
                    {
                        middleware.SetResponseModel(new NewsResponse()
                        {
                            Articles = new List<NewsResponse.ArticleItem>
                            {
                                new NewsResponse.ArticleItem
                                {
                                    Description = "双鱼之恋 - 摄影大赛",
                                    PicUrl = "http://www.bing.com/favicon.ico",
                                    Title = "双鱼之恋 - 摄影大赛",
                                    Url = "http://www.bing.com"
                                }
                            }
                        });
                    }
                })
                //使用InjectTxt方法注入文本消息过滤器
                //只有文本消息的内容是 "我的信息" 的时候才执行这个过滤器
                .InjectTxt((req, middleware) =>
                {
                    if (req.Content == "我的信息")
                    {
                        //获取微信用户信息
                        var user = WeChat.Core.Api.WeChatUserInfo.Get(req.FromUserName);
                        middleware.SetResponseModel(new TextResponse()
                        {
                            Content = String.Format("你的名字是 {0}." + user.NickName),
                            MsgType = WeChat.Core.Utils.Configurations.Current.MessageType.Text
                        });
                    }
                })
                //使用InjectTxt方法注入文本消息过滤器
                //只有文本消息的内容是 "现在时间" 的时候才执行这个过滤器
                .InjectTxt(req => req.Content == "现在时间", req => new TextResponse
                {
                    Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    MsgType = WeChat.Core.Utils.Configurations.Current.MessageType.Text
                })
                //当用户向微信公众号发送图片消息的响应
                .InjectImg((req, middleware) =>
                {
                    var repModel = new NewsResponse()
                    {
                        Articles = new List<NewsResponse.ArticleItem>()
                        {
                            new NewsResponse.ArticleItem()
                            {
                                Description = "Hello world.",
                                PicUrl = "https://www.zhihu.com/favicon.ico",
                                Title = "知乎",
                                Url = "http://www.zhihu.com/"
                            },
                            new NewsResponse.ArticleItem()
                            {
                                Description = "Hello world.",
                                PicUrl = "https://www.bing.com/favicon.ico",
                                Title = "Bing",
                                Url = "http://www.bing.com"
                            }
                        }
                    };

                    middleware.SetResponseModel(repModel);
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
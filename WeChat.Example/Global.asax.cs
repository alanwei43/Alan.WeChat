using System;
using System.IO;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Alan.Log.LogContainerImplement;
using WeChat.Core.Messages.Normal;
using WeChat.Core.Utils;
using WeChat.Example.Library;
using HtmlAgilityPack;

namespace WeChat.Example
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

            //LogUtils.Current.InjectLogModule<DbLog>();
            Alan.Log.LogContainerImplement.LogUtils.Current.InjectLogModule(new Alan.Log.ILogImplement.LogAutoSeperateFiles(100 * 1024, System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/wechat.log")));

            WeChat.Core.Utils.FluentConfig.Get()
                .Inject(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Config.json")) //注入JSON文件的形式传入配置信息
                .Inject(middleware =>
                {
                    //记录请求数据
                    LogUtils.Current.LogWithId(category: "/Message/Request/Data", note: middleware.Input.Request);
                })
                .InjectEnd(middleware =>
                {
                    //记录输出数据
                    LogUtils.Current.LogWithId(category: "/Message/Response/Data", note: middleware.GetResponse());
                })

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
                //只有文本消息的内容是 "现在时间" 的时候才执行这个过滤器
                .InjectTxt(req => req.Content == "现在时间", req => new TextResponse
                {
                    Content = DateTime.Now.ToString("服务器时间 yyyy-MM-dd HH:mm:ss"),
                    MsgType = WeChat.Core.Utils.Configurations.Current.MessageType.Text
                })
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
                .InjectLoc(req => true, (req) => new TextResponse
                {
                    Content = String.Format("你目前所在地是: {0}. ", req.Label),
                    MsgType = WeChat.Core.Utils.Configurations.Current.MessageType.Text
                })
                .InjectEvent(req => req.Event == Configurations.Current.EventType.PickSysPhoto, req => new TextResponse
                {
                    Content = "you select picksysphoto",
                    MsgType = Configurations.Current.MessageType.Text
                })
                .InjectTxt(where: req => req.Content == "cnbeta", setResponse: req =>
                {
                    var url = "http://www.cnbeta.com";
                    WebRequest webReq = WebRequest.Create(url);

                    var allLinks = new List<Tuple<string, string>>();

                    using (var reader = new StreamReader(webReq.GetResponse().GetResponseStream()))
                    {

                        HtmlDocument doc = new HtmlDocument();
                        var html = reader.ReadToEnd();
                        doc.LoadHtml(html);

                        var articles =
                            doc.DocumentNode.SelectNodes("//div[@class]")
                                .FirstOrDefault(l => l.Attributes["class"].Value == "alllist");

                        if (articles == null)
                            return new TextResponse()
                            {
                                Content = "Not found articles",
                                MsgType = Configurations.Current.MessageType.Text
                            };

                        allLinks = articles.SelectNodes("//a[@href]")
                        .Select(art => Tuple.Create(art.InnerText, art.Attributes["href"].Value))
                        .ToList();
                    }



                    var links = from ele in allLinks
                                let matches = Regex.Match(ele.Item2, @"/articles/(\d+)\.htm")
                                where matches.Success && matches.Groups.Count == 2 && !String.IsNullOrWhiteSpace(ele.Item1)
                                select String.Format("{0} {1}", matches.Groups[1].Value, ele.Item1);

                    var rep = new TextResponse()
                    {
                        Content = String.Join(Environment.NewLine, links),
                        MsgType = Configurations.Current.MessageType.Text
                    };

                    return rep;
                })
                .InjectTxt(where: req => Regex.IsMatch(req.Content, @"\d+"), setResponse: req =>
                {
                    var url = String.Format("http://www.cnbeta.com/articles/{0}.htm", req.Content);
                    HtmlDocument doc = new HtmlDocument();
                    WebRequest webReq = WebRequest.Create(url);
                    var reader = new StreamReader(webReq.GetResponse().GetResponseStream());
                    var html = reader.ReadToEnd();
                    doc.LoadHtml(html);

                    var articleContent =
                        doc.DocumentNode.SelectNodes("//section[@class]")
                            .FirstOrDefault(ele => ele.Attributes["class"].Value == "article_content");
                    var textContent = articleContent == null ? "not found" : articleContent.InnerText;

                    var rep = new TextResponse()
                    {
                        Content = textContent,
                        MsgType = Configurations.Current.MessageType.Text
                    };

                    return rep;
                })
                .InjectTxt((req, middleware) => !middleware.SetedResponse, (req, middleware) => new TextResponse
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

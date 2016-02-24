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
using Alan.Log.Core;

namespace WeChat.Example
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

            LogUtils.Current.InjectLogModule<DbLog>();
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
                    if (!middleware.SetedResponse)
                    {
                        middleware.SetResponseModel(new TextResponse()
                        {
                            Content = "未匹配规则",
                            MsgType = Configurations.Current.MessageType.Text
                        });
                    }
                    //记录输出数据
                    LogUtils.Current.LogWithId(category: "/Message/Response/Data", note: middleware.GetResponse());
                })
                .InjectEvent(where: evt => evt.Event == "subscribe", setResponse: evt => new TextResponse()
                {
                    Content = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/directives.txt"))
                })
                //只有文本消息的内容是 "我的信息" 的时候才执行这个过滤器
                .InjectTxt(req => req.Content == "我的信息", req =>
                {
                    //获取微信用户信息
                    var user = WeChat.Core.Api.WeChatUserInfo.Get(req.FromUserName);

                    return new TextResponse()
                    {
                        Content = String.Format("你的名字是 {0}.", Newtonsoft.Json.JsonConvert.SerializeObject(user)),
                        MsgType = WeChat.Core.Utils.Configurations.Current.MessageType.Text
                    };
                })
                //下载图片
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
                .InjectVoice(filter: (req, middle) =>
                {
                    try
                    {
                        var txt = String.IsNullOrWhiteSpace(req.Recognition) ? "empty" : req.Recognition;
                        var rep = new TextResponse
                        {
                            Content = txt,
                            MsgType = Configurations.Current.MessageType.Text
                        };
                        middle.SetResponseModel(rep);
                    }
                    catch (Exception ex)
                    {
                        LogUtils.Current.LogError(id: Guid.NewGuid().ToString(), date: DateTime.Now, message: ex.Message,
                            note: ex.StackTrace, position: ex.Source);
                    }
                })

            #region cnbeta
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

                    var txt = String.Join(Environment.NewLine, links).Trim();
                    txt = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes(txt).Take(2000).ToArray());

                    var rep = new TextResponse()
                    {
                        Content = txt,
                        MsgType = Configurations.Current.MessageType.Text
                    };

                    return rep;

                })
                .InjectTxt(where: req => Regex.IsMatch(req.Content, @"cnbeta \d+"), setResponse: req =>
                {
                    var url = String.Format("http://www.cnbeta.com/articles/{0}.htm", req.Content.Split(' ')[1]);
                    HtmlDocument doc = new HtmlDocument();
                    WebRequest webReq = WebRequest.Create(url);
                    var reader = new StreamReader(webReq.GetResponse().GetResponseStream());
                    var html = reader.ReadToEnd();
                    doc.LoadHtml(html);

                    var articleContent =
                        doc.DocumentNode.SelectNodes("//section[@class]")
                            .FirstOrDefault(ele => ele.Attributes["class"].Value == "article_content");
                    var textContent =
                        (articleContent == null ? "not found" : articleContent.InnerText)
                            .Replace(Environment.NewLine, "")
                            .Trim();

                    var txtBytes = System.Text.Encoding.UTF8.GetBytes(textContent);
                    textContent = System.Text.Encoding.UTF8.GetString(txtBytes.Take(2000).ToArray());
                    var rep = new TextResponse()
                    {
                        Content = textContent,
                        MsgType = Configurations.Current.MessageType.Text
                    };

                    return rep;

                })
            #endregion

                .InjectTxt((req, middleware) => !middleware.SetedResponse, (req, middleware) => new TextResponse
                {
                    Content = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/directives.txt")),
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

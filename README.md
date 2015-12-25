# WeChat
微信自定义消息 自定义菜单等接口封装

# Install

	Install-Package Alan.WeChat

# 全局配置

### 在程序启动时调用的方法


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


### 接收微信推送的消息
	
	下面的代码是一般处理程序

	public class WebHub : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var req = context.Request;
            var rep = context.Response;
            var svr = context.Server;

            if (req.HttpMethod.ToUpper() == "GET")
            {
                rep.Write(req["echostr"]);
                return;
            }

            var responseText = Middleware.Execute(req).GetResponse();
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

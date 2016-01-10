# WeChat
微信自定义消息 自定义菜单等接口封装

# Install

	Install-Package Alan.WeChat

# Introducation
	
首先吐槽一下微信接口写的很烂, 不是排版样式不好, 是文档写的不是很明白.(当然了比我好的不知百倍.)
这个项目从最小的一两个功能, 扩展重构了好几次, 已经实现了[微信接口](http://mp.weixin.qq.com/wiki)的一半了. 包括:

	1. 接收消息
	2. 发送消息
	3. 消息加解密
	4. 素材管理
	5. 用户管理
	6. 自定义菜单
	7. 帐号管理
	8. 微信JS-SDK


这个项目断断续续写了好长时间了, 其实好多抽象类或实体类应该放到更深层的目录下, 最近要忙别的项目了, 这个先告一段落吧. 项目的几个文件夹的大概意思如下:

### Api目录, 主要包括主动调用微信API的接口

	Api.ContentsManage: 微信的素材管理接口, 包括
		

# Use

### 在程序启动时调用的方法


    WeChat.Core.Utils.FluentConfig.Get()
		//.Inject("token", "aes key", "app id", "app secret", Configurations.TransferMode.Cipher)   //以参数的形式传入配置信息
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
	
	下面的代码是在一般处理程序的使用例子: 

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
        public bool IsReusable { get { return false; } }
    }


# API

总共分为两大块: 主动调用微信的接口(获取用户信息, 素材管理, JavaScript SDK配置, 菜单管理， 网页授权), 和接收微信推送的消息/事件.  而且接口调用都同时包含异步和同步两个.

### 获取用户信息

	WeChat.Core.Api.WeChatUserInfo.Get(string openId);
	WeChat.Core.Api.WeChatUserInfo.GetAsync(string openId);

### JavaScript SDK

[官方文档](http://mp.weixin.qq.com/wiki/7/1c97470084b73f8e224fe6d9bab1625b.html)

	WeChat.Core.Api.JsSdk.JsSdkTicket.Get("jsapi", "http://www.YourRequestUrl.com");
	WeChat.Core.Api.JsSdk.JsSdkTicket.GetAsync("jsapi", "http://www.YourRequestUrl.com");

### 创建菜单

[官方文档](http://mp.weixin.qq.com/wiki/6/95cade7d98b6c1e1040cde5d9a2f9c26.html)

从数据库查询, 动态创建菜单:

    Cf_ProjectInfoRepository repProject = new Cf_ProjectInfoRepository();
    var keyMenus = repProject.Query(pro => pro.PIsShow)
        .Select(pro => (new CreateKeyMenuModel(pro.PName.Substring(0, 10), "project_" + pro.PID)) as CreateMenuModel)
        .ToList();

    CreateMenus.Create(new CreateMenusWrapperModel()
    {
        button = new List<CreateBaseMenuModel>
         {
            new CreateKeyMenuModel("我的众筹", "my_projects"),
             new CreateMenuHasSubModel("众筹项目", keyMenus),
             new CreateMenuHasSubModel("其他", new List<CreateMenuModel>
             {
                 new CreateKeyMenuModel("众筹简介", "projects_intro"),
                 new CreateKeyMenuModel("我的参赛作品", "my_photo")
             })
         }
    });

或者传入字典结构的菜单, 也可以直接传入JSON字符串.

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
	

### 删除菜单

	WeChat.Core.Api.MenuManage.DeleteMenus.Delete();
	WeChat.Core.Api.MenuManage.DeleteMenus.DeleteAsync();

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


* Api: 微信接口调用
* Api.ContentsManage: 微信的素材管理接口
* Api.JsSdk: 微信JavaScript SDK相关
* Api.MenuManage: 微信菜单管理
* Api.UserGroupManage: 微信用户管理
* Api.AccessToken.cs: 获取AccessToken
* Api.WebAuth---.cs: 网页授权相关

* Cache: 缓存模块, 主要用于AccessToken的缓存. 这里为了快速开发使用了 System.Web.HttpContext.Current.Cache 对象来管理缓存.
* EncryptDecrypt: 加密/解密模块, 这里直接使用了腾讯给的示例代码.
* Log: 日志记录模块.

* Messages: 自定义消息模块, 这里利用中间件的思想. 

所有的接口使用方式大都类似, 而且都提供同步和异步两种方式, 文档的使用示例小节有例子.

后来为了使用方便, 创建了一个 FluentConfig 类, 你可以在应用程序启动的时候调用这个类, 来初始化配置, 注入日志/缓存模块, 拦截微信推送的消息, 并相应微信推送的消息.


# Configuration

有两种配置方式, 一种是写配置文件读取, 一种是调用方法配置.
配置文件(*假设文件路径是: E:\WeChat\App_Data\config.json*)的格式如下:

	{
	  "Token": "token",
	  "AESKey": "AES key",
	  "AppId": "App Id",
	  "AppSecret": "App Secret",
	  "MessageModel": "消息模式: Plain(明文)/Hybrid(混合)/Cipher(加密)"
	}

可以使用如下代码修改配置:

	WeChat.Core.Utils.Configurations.Inject(System.IO.File.ReadAllText(@"E:\WeChat\App_Data\config.json"));
	
获取使用如下方式修改配置: 

	WeChat.Core.Utils.Configurations.Inject(new Configurations()
            {
                Token = token,
                AesKey = aesKey,
                AppId = appId,
                AppSecret = appSecret,
                EnumMessageMode = mode
            });

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


### 响应微信推送的消息
	
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
				//如果是GET请求则视为微信配置时的服务器校验
                rep.Write(req["echostr"]);
                return;
            }

            var responseText = Middleware.Execute(req).GetResponse();
            rep.Write(responseText);
        }
        public bool IsReusable { get { return false; } }
    }



## API使用示例

### 根据获取微信用户信息

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




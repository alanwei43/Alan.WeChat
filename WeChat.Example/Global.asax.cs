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
                if (click.EventKey == "projects")
                {
                    var newsRep = new NewsResponse()
                    {
                        ToUserName = click.FromUserName,
                        FromUserName = click.ToUserName,
                        Articles = new List<NewsResponse.ArticleItem>()
                         {
                              new NewsResponse.ArticleItem()
                              {
                                  Url = "https://zc.yupen.cn/CrowdFunding/Show/51",
                                  Description = "为向客户提供绿色、健康的养生茶，公司从基地建立之初就严守有机茶种植标准，开垦未种植过作物的荒山作为基地，保证土壤无污染；苗木种植至今五年间从未使用农药、化肥、除草剂等，保证种出的茶叶无污染；选择土壤中富含硒锌等微量元素的丹寨，以打造真正的绿色健康养生的好茶。",
                                  PicUrl = "https://zcapi.yupen.cn/Static/Attachments/c66074e7-ca3c-4ad6-8455-c3d0c0797cc9.jpg",
                                  Title ="产自贵州高海拔地区无任何空气污染与仙界为邻的茶——硒鋅茶"
                              },
                              new NewsResponse.ArticleItem()
                              {
                                   Url="https://zc.yupen.cn/CrowdFunding/Show/50",
                                   Description="产自高海拔 无公害基地,其米质晶莹如玉,光洁透明,香气浓郁.从视觉上就让你食欲大振的米——硒锌米，让你体会啥叫吃嘛嘛香的感觉！",
                                   PicUrl = "https://zcapi.yupen.cn/Static/Attachments/7464315a-3b6e-4bc1-82a6-4b29f5f626ae.jpg",
                                   Title="产自贵州高海拔地区无任何空气污染与仙界为邻的米——硒鋅米"
                              },
                              new NewsResponse.ArticleItem()
                              {
                                   Url="https://zc.yupen.cn/CrowdFunding/Show/48",
                                   Description="「三期」保护传统文化 耕耘锦绣中华——“绣娘学校”众筹项目",
                                   PicUrl = "https://zcapi.yupen.cn/Static/Attachments/a3e6c7f6-359d-4b26-9886-f7e56ad9878e.jpg",
                                   Title="2006年，苗绣经国务院批准列入第一批国家级非物质文化遗产名录。华澳融信希望筹办“绣娘学校”，将中国苗绣的丝线艺术展现给世人！让苗绣传承下去需要全民族的努力和支持，我们期待，与您一起共同编织锦绣中华！"
                              }
                         }
                    };
                    middleware.SetResponseModel(newsRep);
                }
            });

            Middleware.ClickFilters.Inject((click, middleware) =>
            {
                if (click.EventKey == "project_intro")
                {
                    var txtRep = new TextResponse()
                    {
                        Content = "北京华澳翼时代信息技术有限责任公司（后简称：翼时代）创建于2014年，是一家信用数据整合服务、咨询、业务扩展于一体的综合性现代金融服务公司。而且不断探索新型的小微借款服务模式，建立了个人对个人小微借款交易促成平台。目前平台已经成功促成数万笔借贷交易，为两端客户提供了专业、高效的支持服务，促进了社会诚信体系建设与社会和谐。",
                        ToUserName = click.FromUserName,
                        FromUserName = click.ToUserName,
                        MsgType = Configurations.Current.MessageType.Text
                    };
                    middleware.SetResponseModel(txtRep);
                }
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
            CreateMenus.Create(new CreateMenusWrapperModel()
            {
                 button = new List<CreateBaseMenuModel>
                 {
                     new CreateLinkMenuModel("", ""),
                     new CreateMenuHasSubModel("", new List<CreateMenuModel>()
                     {
                         new CreateLinkMenuModel("", "")
                     })
                      
                 }
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
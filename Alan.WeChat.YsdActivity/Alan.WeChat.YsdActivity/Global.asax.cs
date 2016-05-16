using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;
using Alan.Log.Core;
using Alan.Log.LogContainerImplement;
using WeChat.Core.Messages;
using WeChat.Core.Messages.Normal;
using WeChat.Core.Utils;
using System.Text.RegularExpressions;
using WeChat.YsdActivity.Library.ExUtils;
using WeChat.YsdActivity.Library;
using WeChat.YsdActivity.Models;
using Dapper;
using WeChat.Core.Messages.Events;
using WeChat.Core.Messages.Middlewares;

namespace WeChat.YsdActivity
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            var configFilePath = HostingEnvironment.MapPath("~/App_Data/Config.json");

            Alan.Log.LogContainerImplement.LogUtils.Current.InjectLogModule(new Alan.Log.Bmob.LogBmob("Logs", "b61048de9001d146307bdd704e87c59b", "fa1198d5867bdce5799a813c2933f420"));

            WeChat.Core.Utils.FluentConfig.Get()
              .Inject(configFilePath)
              .Inject(req =>
              {
                  req.SetResponseModel(new TextResponse
                  {
                      Content = "received",
                      MsgType = Configurations.Current.MessageType.Text
                  });
                  LogUtils.Current.LogInfo(id: Guid.NewGuid().ToString(), date: DateTime.Now, category: "request", request: req.Input.Request);
              })

              .InjectEnd(req =>
              {
                  LogUtils.Current.LogInfo(id: Guid.NewGuid().ToString(), date: DateTime.Now, category: "response", response: req.GetResponse());
              })
              .Inject((EventBase req, MiddlewareParameter middleware) =>
              {
                  if (req.Event == "pic_sysphoto")
                  {
                      var pickPhoto = middleware.Input.GetRequestModel<PickPhotoRequest>();
                      if (pickPhoto == null) return;
                  }
              })

              .InjectVoice((req, middleware) =>
              {
                  middleware.SetResponseModel(
                  new TextResponse
                  {
                      Content = String.Join(Environment.NewLine, new[] {
                          String.Format("抱歉, 你发送的语音消息({0})没有匹配到任何指令.", req.Recognition),
                          "发送语音: \"选手列表\" 可以获取选手列表及得票数量 ",
                          "发送语音: \"投票XX号\" 可以为XX号选手投票" }),
                      MsgType = Configurations.Current.MessageType.Text
                  });
              })

              .InjectVoice(where: req => req.Recognition.StartsWith("选手列表"), setResponse: req =>
                {
                    using (var connection = SqlUtils.GetConnection())
                    {
                        var players = connection.Query<Player>("select p.*, (select COUNT(*) from Votes where PlayerId = p.Id) as VotedCount from Players p ").OrderByDescending(p => p.VotedCount);
                        var content = "编号 / 姓名 / 票数" + Environment.NewLine;
                        return new TextResponse
                        {
                            Content = content + String.Join(Environment.NewLine, players.Select(p => String.Format("{0} / {1} / {2}", p.Id, p.Name, p.VotedCount))),
                            MsgType = Configurations.Current.MessageType.Text
                        };
                    }
                })
              .InjectTxt(where: req => req.Content == "选手列表", setResponse: req =>
                {
                    using (var connection = SqlUtils.GetConnection())
                    {
                        var players = connection.Query<Player>("select p.*, (select COUNT(*) from Votes where PlayerId = p.Id) as VotedCount from Players p ");
                        var content = "编号 / 姓名 / 票数" + Environment.NewLine;
                        return new TextResponse
                        {
                            Content = content + String.Join(Environment.NewLine, players.Select(p => String.Format("{0} / {1} / {2}", p.Id, p.Name, p.VotedCount))),
                            MsgType = Configurations.Current.MessageType.Text
                        };
                    }
                })
              .InjectTxt(where: req => Regex.IsMatch(req.Content, @"^\u6295\u7968(.+)\u53f7"), setResponse: req =>
              {

                  var match = Regex.Match(req.Content, @"^\u6295\u7968(.+)\u53f7");
                  if (match.Groups.Count < 2)
                  {
                      return new TextResponse
                      {
                          Content = String.Format("你发送的文本未能正确匹配({0}). 比如你要为18号选手投票, 发送文本: \"投票18号\" 就可以为18号选手投票.", req.Content),
                          MsgType = Configurations.Current.MessageType.Text
                      };
                  }
                  var capturedValue = match.Groups[1].Value;

                  var numberValue = Regex.IsMatch(capturedValue, @"^\d{1,2}$") ? int.Parse(capturedValue) : capturedValue.ConvertChineseNumberToInt();

                  var txt = "";
                  if (numberValue == -1)
                  {
                      txt = "选手号数字限制在0-99. 比如你要为18号选手投票, 发送文本: \"投票十八号\" 即可.";
                  }
                  else
                  {
                      txt = String.Format("你已为{0}号选手投票成功.", numberValue);
                  }

                  return new WeChat.Core.Messages.Normal.TextResponse
                  {
                      Content = txt,
                      MsgType = Configurations.Current.MessageType.Text
                  };
              })
              .InjectVoice(where: req => Regex.IsMatch(req.Recognition, @"^\u6295\u7968(.+)\u53f7"), setResponse: req =>
              {
                  var match = Regex.Match(req.Recognition, @"^\u6295\u7968(.+)\u53f7");
                  if (match.Groups.Count < 2)
                  {
                      return new TextResponse
                      {
                          Content = String.Format("你发送的语音未能正确匹配({0}). 比如你要为18号选手投票, 发送语音: \"投票十八号\" 就可以为18号选手投票.", req.Recognition),
                          MsgType = Configurations.Current.MessageType.Text
                      };
                  }

                  var numberValue = match.Groups[1].Value.ConvertChineseNumberToInt();
                  var txt = "";
                  if (numberValue == -1)
                  {
                      txt = "选手号数字限制在0-99. 比如你要为18号选手投票, 发送语音: \"投票十八号\" 即可.";
                  }
                  else
                  {

                      Vote v = new Vote
                      {
                          PlayerId = numberValue,
                          PlayerOpenId = numberValue.ToString(),
                          VoterOpenId = req.FromUserName
                      };
                      v.Insert();

                      txt = String.Format("你已为{0}号选手投票成功.", numberValue);
                  }

                  return new WeChat.Core.Messages.Normal.TextResponse
                  {
                      Content = txt,
                      MsgType = Configurations.Current.MessageType.Text
                  };
              })
              .InjectVoice(where: req => req.Recognition.StartsWith("我的信息"), setResponse: req =>
                {
                    //return new NewsResponse
                    //{
                    //    Articles = new List<NewsResponse.ArticleItem>
                    //     {
                    //          new NewsResponse.ArticleItem
                    //          {
                    //               Description = "",
                    //                Title = "Alan",
                    //                 PicUrl = "",
                    //                  Url=""
                    //          }
                    //     }
                    //}

                    var count = SqlUtils.GetConnection().ExecuteScalar<string>("select count(*) from Votes where PalyerId = (select Id from Players where WeChatOpenId = @openId)", new { openId = req.FromUserName });

                    return new TextResponse
                    {
                        Content = "你的投票数是: " + count,
                        MsgType = Configurations.Current.MessageType.Text
                    };
                })
              .InjectImg(where: req => true, setResponse: req =>
              {
                  var response = HttpUtils.Get(req.PicUrl, null, "GET").DownloadFile();
                  var filePath = HostingEnvironment.MapPath("~/Contents/Avatars/" + response.Item1);
                  File.WriteAllBytes(filePath, response.Item2);

                  var weChatUser = WeChat.Core.Api.WeChatUserInfo.Get(req.FromUserName);

                  var player = new Player
                  {
                      Avatar = filePath,
                      Name = weChatUser.NickName,
                      WeChatOpenId = req.FromUserName,
                      WeChatAvatar = weChatUser.HeadImgUrl,
                      WeChatName = weChatUser.NickName
                  };
                  player.Insert();

                  return new TextResponse()
                  {
                      Content = req.PicUrl,
                      MsgType = Configurations.Current.MessageType.Text
                  };
              });

            SqlUtils.Init();
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
            var app = sender as HttpApplication;
            var ex = app.Server.GetLastError();
            LogUtils.Current.LogError(id: Guid.NewGuid().ToString(), date: DateTime.Now, category: "Application_Error", message: ex.Message, position: ex.StackTrace, note: ex.Source);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
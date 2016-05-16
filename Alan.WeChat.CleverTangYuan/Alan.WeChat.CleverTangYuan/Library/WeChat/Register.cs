using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChat.Core.Utils;
using WeChat.Core.Messages;
using WeChat.Core.Messages.Normal;
using WeChat.Core.Messages.Events;
using Alan.Log.LogContainerImplement;
using Alan.Log.Core;
using Alan.Utils.ExtensionMethods;
using Alan.WeChat.CleverTangYuan.Models;

namespace Alan.WeChat.CleverTangYuan.Library.WeChat
{
    public class Register
    {
        public static void Run()
        {
            FluentConfig.Get()
                .Inject(Config.Current.Token, Config.Current.AESKey, Config.Current.AppId, Config.Current.AppSecret, Configurations.TransferMode.Plain)
                .InjectPre(middle =>
                {
                    LogUtils.Current.LogDebug(category: "/WeChat/Register", message: middle.Input.RawRequest);
                    var txtReq = middle.Input.GetRequestModel<TextRequest>();
                    if (txtReq != null)
                    {
                        LogUtils.Current.LogDebug(message: "text request content: " + txtReq.Content, category: "/WeChat/Register");
                    }

                    Models.WeChatUser checkUser = new Models.WeChatUser()
                    {
                        OpenId = middle.Input.RequestBaseModel.FromUserName
                    };
                    checkUser.InsertIfNotExist();
                })
                .InjectTxt(req => req.Content == "query", req =>
                {
                    LogUtils.Current.LogDebug(message: "match query");
                    var model = new Models.WeChatUserRecord();
                    var response = model.Query(req.FromUserName, 0, 50).ToTextResponse();
                    LogUtils.Current.LogDebug(message: "match query: " + response.ExToJson());
                    return response;
                })
                .InjectTxt(req => System.Text.RegularExpressions.Regex.IsMatch(req.Content, @"^query \d+ \d+$"), req =>
                {

                    var match = System.Text.RegularExpressions.Regex.Match(req.Content, @"^query (\d+) (\d+)$");
                    var skip = match.Groups[1].Value.ExToInt();
                    var take = match.Groups[2].Value.ExToInt();

                    var model = new Models.WeChatUserRecord();
                    return model.Query(req.FromUserName, skip, take).ToTextResponse();
                })
                .InjectTxt(where: (req, middle) => System.Text.RegularExpressions.Regex.IsMatch(req.Content, @"^query .+$"), setResponse: (req, middle) =>
                {
                    if (middle.SetedResponse) return;

                    var match = System.Text.RegularExpressions.Regex.Match(req.Content, @"^query (.+)$");
                    var keywords = match.Groups[1].Value;

                    var query = new Models.WeChatUserRecord();
                    return query.Query(req.FromUserName, keywords).ToTextResponse();
                })
                .InjectTxt((req, middle) =>
                {
                    if (middle.SetedResponse) return;
                    Models.WeChatUserRecord record = new Models.WeChatUserRecord()
                    {
                        OpenId = req.FromUserName,
                        Content = req.Content
                    };
                    record.Push();

                    middle.SetResponseModel(new TextResponse()
                    {
                        Content = record.Content,
                        MsgType = Configurations.Current.MessageType.Text
                    });
                })
                .InjectEnd(middle =>
                {
                    if (middle.SetedResponse) return;
                    middle.SetResponseModel(new TextResponse()
                    {
                        Content = "暂不支持此类型",
                        MsgType = Configurations.Current.MessageType.Text
                    });
                });
        }
    }
}
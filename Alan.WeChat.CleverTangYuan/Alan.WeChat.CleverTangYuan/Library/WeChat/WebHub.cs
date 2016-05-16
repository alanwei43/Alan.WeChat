using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WeChat.Core.Messages.Middlewares;
using Alan.Log.Core;
using Alan.Log.LogContainerImplement;
using Alan.Utils.ExtensionMethods;

namespace Alan.WeChat.CleverTangYuan.Library.WeChat
{
    /// <summary>
    /// 微信Hub
    /// </summary>
    public class WebHub : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            var req = context.Request;
            var rep = context.Response;
            var svr = context.Server;

            try
            {
                LogUtils.Current.LogDebug(message: String.Format("{0} {1}.", req.HttpMethod, req.RawUrl));

                if (req.HttpMethod.ToUpper() == "GET")
                {
                    rep.ContentEncoding = Encoding.UTF8;
                    var echo = req["echostr"];
                    rep.Write(echo);
                    return;
                }

                var responseTxt = Middleware.Execute(req).GetResponse();

                rep.Write(responseTxt);
            }
            catch (Exception ex)
            {
                LogUtils.Current.LogError(date: DateTime.Now, message: String.Format("{0}", ex.Message));

            }
        }
    }
}
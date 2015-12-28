using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChat.Core.Api;

namespace WeChat.Example.Api
{
    /// <summary>
    /// Summary description for GetAccessToken
    /// </summary>
    public class GetAccessToken : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(AccessToken.Get().Access_Token);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
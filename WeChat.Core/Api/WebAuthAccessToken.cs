using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.Core.Utils;

namespace WeChat.Core.Api
{
    public class WebAuthAccessToken : ApiBase
    {
        private readonly string _code;

        /// <summary>
        /// 网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        /// </summary>
        public string Access_Token { get; set; }

        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public long Expires_In { get; set; }

        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string Refresh_Token { get; set; }

        /// <summary>
        /// 用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// 当且仅当该公众号已获取用户的userinfo授权，并且该公众号已经绑定到微信开放平台帐号时，才会出现该字段
        /// </summary>
        public string UnionId { get; set; }


        protected async override Task<string> GetApiUrlAsync()
        {
            return
                await Task.FromResult(
                String.Format(
                    "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                    Configurations.Current.AppId, Configurations.Current.AppSecret, this._code));

        }

        protected override string GetApiUrl()
        {
            return String.Format(
                "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                Configurations.Current.AppId, Configurations.Current.AppSecret, this._code);
        }

        public WebAuthAccessToken() { }

        public WebAuthAccessToken(string code)
        {
            this._code = code;
        }

        private static string GetCacheKey(string code)
        {
            return "/WeChat/WebAuthToken/AccessToken/" + code;
        }

        /// <summary>
        /// 通过code换取网页授权access_token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static WebAuthAccessToken GetAccessToken(string code)
        {
            var webAuth = new WebAuthAccessToken(code);
            var response = webAuth.RequestAsModel<WebAuthAccessToken>();
            return response;

        }

        /// <summary>
        /// 通过code换取网页授权access_token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static async Task<WebAuthAccessToken> GetAccessTokenAsync(string code)
        {
            var webAuth = new WebAuthAccessToken(code);
            var response = await webAuth.RequestAsModelAsync<WebAuthAccessToken>();
            return response;

        }

    }
}

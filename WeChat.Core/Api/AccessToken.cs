using System;
using System.Net;
using System.Threading.Tasks;
using WeChat.Core.Utils;

namespace WeChat.Core.Api
{
    /// <summary>
    /// 获取access token
    /// </summary>
    public class AccessToken : ApiBase
    {
        protected async override Task<string> GetApiUrlAsync()
        {
            return await Task.FromResult(String.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", Configurations.Current.AppId, Configurations.Current.AppSecret));
        }

        protected override string GetApiUrl()
        {
            return String.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", Configurations.Current.AppId, Configurations.Current.AppSecret);
        }


        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string Access_Token { get; set; }

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public long Expires_In { get; set; }


        public async static Task<AccessToken> GetAsync()
        {
            var accessToken = CacheUtils.Get("/WeChat/AccessToken");
            if (accessToken == null)
            {
                var at = new AccessToken();
                var token = await at.RequestAsModelAsync<AccessToken>();
                if (token.ErrCode.GetValueOrDefault() != 0) throw new Exception(String.Format("获取access_token失败, 错误码: {0}, 错误信息: {1}.", token.ErrCode, token.ErrMsg));
                CacheUtils.Add("/WeChat/AccessToken", token, DateTime.Now.AddSeconds(at.Expires_In - 1200));
                return token;
            }

            return (AccessToken)accessToken;
        }

        public static AccessToken Get()
        {
            var accessToken = CacheUtils.Get("/WeChat/AccessToken");
            if (accessToken == null)
            {
                var at = new AccessToken();
                var token = at.RequestAsModel<AccessToken>();
                if (token.ErrCode.GetValueOrDefault() != 0) throw new Exception(String.Format("获取access_token失败, 错误码: {0}, 错误信息: {1}.", token.ErrCode, token.ErrMsg));
                CacheUtils.Add("/WeChat/AccessToken", token, DateTime.Now.AddSeconds(at.Expires_In - 1200));
                return token;
            }

            return (AccessToken)accessToken;
        }
    }
}

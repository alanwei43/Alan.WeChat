using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api
{
    public class WebAuthUserInfo : ApiBase
    {
        private string _code;
        /// <summary>
        /// 用户的唯一标识
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 用户个人资料填写的省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 普通用户个人资料填写的城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 国家，如中国为CN
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。
        /// </summary>
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）
        /// </summary>
        public List<string> Privilege { get; set; }
        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。详见：
        /// </summary>
        public string UnionId { get; set; }

        protected async override Task<string> GetApiUrlAsync()
        {
            var accessToken = await WebAuthAccessToken.GetAccessTokenAsync(this._code);
            if (accessToken.ErrCode != null && accessToken.ErrCode.Value != 0) throw new Exception(String.Format("获取网页授权access_token失败: {0} {1}.", accessToken.ErrCode, accessToken.ErrMsg));

            return String.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", accessToken.Access_Token, accessToken.OpenId);

        }

        protected override string GetApiUrl()
        {
            var accessToken = WebAuthAccessToken.GetAccessToken(this._code);
            if (accessToken.ErrCode != null && accessToken.ErrCode.Value != 0) throw new Exception(String.Format("获取网页授权access_token失败: {0} {1}.", accessToken.ErrCode, accessToken.ErrMsg));

            return String.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", accessToken.Access_Token, accessToken.OpenId);
        }
        public WebAuthUserInfo() { }
        public WebAuthUserInfo(string code) { }

        /// <summary>
        /// 拉取用户信息(需scope为 snsapi_userinfo)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static WebAuthUserInfo Get(string code)
        {
            var user = new WebAuthUserInfo(code);
            var response = user.RequestAsModel<WebAuthUserInfo>();
            return response;
        }

        /// <summary>
        /// 拉取用户信息(需scope为 snsapi_userinfo)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static async Task<WebAuthUserInfo> GetAsync(string code)
        {
            var user = new WebAuthUserInfo(code);
            var response = await user.RequestAsModelAsync<WebAuthUserInfo>();
            return response;
        }
    }
}

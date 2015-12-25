using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api
{
    /// <summary>
    /// 获取微信用户信息
    /// </summary>
    public class WeChatUserInfo : ApiBase
    {
        public WeChatUserInfo() { }

        public WeChatUserInfo(string openId)
        {
            this.OpenId = openId;
        }
        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        public string OpenId { get; set; }

        protected async override Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            var url = String.Format(
                "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", token.Access_Token, this.OpenId);
            return url;
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            var url = String.Format(
                "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", token.Access_Token, this.OpenId);
            return url;
        }

        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        /// </summary>
        public int Subscribe { get; set; }

        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender
        {
            get
            {
                return this.Sex == 1 ? "男" : (this.Sex == 2 ? "女" : "未知");
            }
        }
        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 用户的语言，简体中文为zh_CN
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。
        /// </summary>
        public string HeadImgUrl { get; set; }

        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>
        public long Subscribe_Time { get; set; }

        public DateTime SubscribeTime
        {
            get
            {
                var date1970 = new DateTime(1970, 1, 1);
                return date1970.AddSeconds(this.Subscribe_Time);
            }
        }
        /// <summary>
        /// 公众号运营者对粉丝的备注，公众号运营者可在微信公众平台用户管理界面对粉丝添加备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 用户所在的分组ID
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。详见：获取用户个人信息（UnionID机制）
        /// </summary>
        public string UnionId { get; set; }

        public async static Task<WeChatUserInfo> GetAsync(string openId)
        {
            var weChatUser = new WeChatUserInfo(openId);
            var response = await weChatUser.RequestAsModelAsync<WeChatUserInfo>();
            return response;
        }
        public static WeChatUserInfo Get(string openId)
        {
            var weChatUser = new WeChatUserInfo(openId);
            var response = weChatUser.RequestAsModel<WeChatUserInfo>();
            return response;
        }
    }
}

using Alan.Utils.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.MenuManage
{
    /// <summary>
    /// 测试个性化菜单匹配结果
    /// </summary>
    public class TestPersonalMenus : ApiBase
    {
        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (!token.IsSuccess) throw new Exception(String.Format("测试个性化菜单匹配结果时获取AccessToken失败: {0} {1}.", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/menu/trymatch?access_token=ACCESS_TOKEN", token.Access_Token);
        }

        protected override async Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (!token.IsSuccess) throw new Exception(String.Format("测试个性化菜单匹配结果时获取AccessToken失败: {0} {1}.", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/menu/trymatch?access_token=ACCESS_TOKEN", token.Access_Token);
        }
        public TestPersonalMenus() { }
        /// <summary>
        /// 测试个性化菜单匹配结果
        /// </summary>
        /// <param name="userId">user_id可以是粉丝的OpenID 也可以是粉丝的微信号</param>
        public TestPersonalMenus(int userId)
        {
            this.ReqData = Encoding.UTF8.GetBytes(new { user_id = userId }.ExToJson());
        }
        /// <summary>
        /// 测试个性化菜单匹配结果
        /// </summary>
        /// <param name="userId">user_id可以是粉丝的OpenID 也可以是粉丝的微信号</param>
        /// <returns></returns>
        public static TestPersonalMenus Test(int userId)
        {
            var test = new TestPersonalMenus(userId);
            return test.RequestAsModel<TestPersonalMenus>();
        }
        /// <summary>
        /// 测试个性化菜单匹配结果
        /// </summary>
        /// <param name="userId">user_id可以是粉丝的OpenID 也可以是粉丝的微信号</param>
        /// <returns></returns>
        public static async Task<TestPersonalMenus> TestAsync(int userId)
        {
            var test = new TestPersonalMenus(userId);
            return await test.RequestAsModelAsync<TestPersonalMenus>();
        }
    }
}

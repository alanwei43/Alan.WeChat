using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Api;

namespace WeChat.Core.Api
{
    /// <summary>
    /// 自定义菜单创建接口
    /// </summary>
    public class CreateMenus : ApiBase
    {
        protected async override Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            return String.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", token.Access_Token);
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            return String.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", token.Access_Token);
        }

        protected override string ReqMethod { get { return "POST"; } }

        protected override string ReqData { get; set; }
        public CreateMenus() { }

        public CreateMenus(CreateMenusWrapperModel createMenusWrapper)
        {
            this.ReqData = createMenusWrapper.ExToJson();
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="createMenusWrapper"></param>
        /// <returns></returns>
        public async static Task<CreateMenus> GetAsync(CreateMenusWrapperModel createMenusWrapper)
        {
            var create = new CreateMenus(createMenusWrapper);
            var response = await create.SendRequestAsync<CreateMenus>();
            return response;
        }

    }

}

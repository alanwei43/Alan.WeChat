using System;
using System.Threading.Tasks;
using Alan.Utils.ExtensionMethods;

namespace WeChat.Core.Api.MenuManage
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
        public async static Task<CreateMenus> CreateAsync(CreateMenusWrapperModel createMenusWrapper)
        {
            var create = new CreateMenus(createMenusWrapper);
            var response = await create.SendRequestAsync<CreateMenus>();
            return response;
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="createMenusWrapper"></param>
        /// <returns></returns>
        public static CreateMenus Create(CreateMenusWrapperModel createMenusWrapper)
        {
            var create = new CreateMenus(createMenusWrapper);
            var response = create.SendRequest<CreateMenus>();
            return response;
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="menus">菜单JSON数据</param>
        /// <returns></returns>
        public static CreateMenus Create(string menus)
        {
            var menu = new CreateMenus();
            menu.ReqData = menus;
            return menu.SendRequest<CreateMenus>();
        }

    }

}

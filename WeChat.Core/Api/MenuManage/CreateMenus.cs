using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Api.Models;
using WeChat.Core.Api.MenuManage.Models;

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

        protected override byte[] ReqData { get; set; }
        public CreateMenus() { }

        public CreateMenus(CreateMenusWrapperModel createMenusWrapper)
        {
            this.ReqData = Encoding.UTF8.GetBytes(createMenusWrapper.ExToJson());
        }
        public CreateMenus(string menuJson)
        {
            this.ReqData = Encoding.UTF8.GetBytes(menuJson);
        }

        public CreateMenus(Dictionary<string, object> menus)
        {
            this.ReqData = Encoding.UTF8.GetBytes(menus.ExToJson());
        }


        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="createMenusWrapper"></param>
        /// <returns></returns>
        public async static Task<CreateMenus> CreateAsync(CreateMenusWrapperModel createMenusWrapper)
        {
            var create = new CreateMenus(createMenusWrapper);
            var response = await create.RequestAsModelAsync<CreateMenus>();
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
            var response = create.RequestAsModel<CreateMenus>();
            return response;
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="menus">菜单JSON数据</param>
        /// <returns></returns>
        public static CreateMenus Create(string menus)
        {
            var menu = new CreateMenus(menus);
            return menu.RequestAsModel<CreateMenus>();
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="menus">菜单JSON数据</param>
        /// <returns></returns>
        public async static Task<CreateMenus> CreateAsync(string menus)
        {
            var menu = new CreateMenus(menus);
            return await menu.RequestAsModelAsync<CreateMenus>();
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="menus">菜单JSON数据</param>
        /// <returns></returns>
        public static CreateMenus Create(Dictionary<string, object> menus)
        {
            var menu = new CreateMenus(menus);
            return menu.RequestAsModel<CreateMenus>();
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="menus">菜单JSON数据</param>
        /// <returns></returns>
        public async static Task<CreateMenus> CreateAsync(Dictionary<string, object> menus)
        {
            var menu = new CreateMenus(menus);
            return await menu.RequestAsModelAsync<CreateMenus>();
        }

    }

}

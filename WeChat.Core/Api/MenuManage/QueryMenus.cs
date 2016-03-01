using System;
using System.Threading.Tasks;
using WeChat.Core.Api.MenuManage.Models;
using WeChat.Core.Api.Models;

namespace WeChat.Core.Api.MenuManage
{
    public class QueryMenus : ApiBase
    {
        protected async override Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            var url = String.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", token.Access_Token);
            return url;
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            var url = String.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", token.Access_Token);
            return url;
        }

        public QueryMenusWrapperModel menu { get; set; }

        /// <summary>
        /// 获取查询菜单
        /// </summary>
        /// <returns></returns>
        public static QueryMenus Get()
        {
            var query = new QueryMenus();
            var menus = query.RequestAsModel<QueryMenus>();
            return menus;
        }

        /// <summary>
        /// 异步获取查询菜单
        /// </summary>
        /// <returns></returns>
        public async static Task<QueryMenus> GetAsync()
        {
            var query = new QueryMenus();
            var menus = await query.RequestAsModelAsync<QueryMenus>();
            return menus;
        }
    }

}

using System;
using System.Threading.Tasks;

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
        public async static Task<QueryMenus> GetAsync()
        {
            var query = new QueryMenus();
            var menus = await query.SendRequestAsync<QueryMenus>();
            return menus;
        }
    }

}

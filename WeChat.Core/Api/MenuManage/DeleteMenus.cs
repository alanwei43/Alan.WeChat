using System;
using System.Threading.Tasks;

namespace WeChat.Core.Api.MenuManage
{
    public class DeleteMenus : ApiBase
    {
        protected async override Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            var url = String.Format("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", token.Access_Token);
            return url;
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            var url = String.Format("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", token.Access_Token);
            return url;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <returns></returns>
        public static DeleteMenus Delete()
        {
            var deleteMenu = new DeleteMenus();
            var response = deleteMenu.RequestAsModel<DeleteMenus>();
            return response;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <returns></returns>
        public async static Task<DeleteMenus> DeleteAsync()
        {
            var deleteMenu = new DeleteMenus();
            var response = await deleteMenu.RequestAsModelAsync<DeleteMenus>();
            return response;
        }
    }
}

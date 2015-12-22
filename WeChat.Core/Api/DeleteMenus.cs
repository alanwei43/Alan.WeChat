using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api
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

        public async static Task<DeleteMenus> GetAsync()
        {
            var deleteMenu = new DeleteMenus();
            var response = await deleteMenu.SendRequestAsync<DeleteMenus>();
            return response;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api
{
    /// <summary>
    /// 获取素材列表
    /// </summary>
    public class QueryContents : ApiBase
    {
        protected async override Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            return String.Format("https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token={0}", token.Access_Token);
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            return String.Format("https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token={0}", token.Access_Token);
        }

        public async static Task<string> GetAsync()
        {
            var query = new QueryContents();
            var response = await query.SendRequestAsync();
            return response;
        }
        public static string Get()
        {
            var query = new QueryContents();
            var response = query.SendRequest();
            return response;
        }
    }
}

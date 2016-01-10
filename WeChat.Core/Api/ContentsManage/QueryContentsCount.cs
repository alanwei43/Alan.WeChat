using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.Core.Api.Models;

namespace WeChat.Core.Api.ContentsManage
{
    public class QueryContentsCount : ApiBase
    {
        protected async override Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            return String.Format("https://api.weixin.qq.com/cgi-bin/material/get_materialcount?access_token={0}", token.Access_Token);
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            return String.Format("https://api.weixin.qq.com/cgi-bin/material/get_materialcount?access_token={0}", token.Access_Token);
        }

        /// <summary>
        /// 语音总数量
        /// </summary>
        public long Voice_Count { get; set; }

        /// <summary>
        /// 视频总数量
        /// </summary>
        public long Video_Count { get; set; }

        /// <summary>
        /// 图片总数量
        /// </summary>
        public long Image_Count { get; set; }

        /// <summary>
        /// 图文总数量
        /// </summary>
        public long News_Count { get; set; }

        /// <summary>
        /// 获取素材总数
        /// </summary>
        /// <returns></returns>
        public static QueryContentsCount Query()
        {
            var query = new QueryContentsCount();
            var response = query.RequestAsModel<QueryContentsCount>();
            return response;
        }

        /// <summary>
        /// 获取素材总数
        /// </summary>
        /// <returns></returns>
        public static async Task<QueryContentsCount> QueryAsync()
        {
            var query = new QueryContentsCount();
            var response = await query.RequestAsModelAsync<QueryContentsCount>();
            return response;
        }
    }
}

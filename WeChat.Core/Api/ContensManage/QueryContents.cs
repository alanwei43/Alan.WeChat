using System;
using System.Threading.Tasks;
using Alan.Utils.ExtensionMethods;

namespace WeChat.Core.Api.ContensManage
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

        protected override string ReqMethod { get { return "POST"; } }

        public QueryContents(string type, int offset, int count)
        {
            this.ReqData = new
            {
                type,
                offset,
                count
            }.ExToJson();
        }

        private async static Task<string> GetAsync(string type, int offset, int count)
        {
            var query = new QueryContents(type, offset, count);
            var response = await query.RequestAsStringAsync();
            return response;
        }
        private static string Get(string type, int offset, int count)
        {
            var query = new QueryContents(type, offset, count);
            var response = query.RequestAsString();
            return response;
        }

        /// <summary>
        /// 获取图文素材列表
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static NewsContentsModel GetNews(int offset, int count)
        {
            var response = Get("news", offset, count);
            return response.ExJsonToEntity<NewsContentsModel>();
        }

        /// <summary>
        /// 异步获取图文素材列表
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async static Task<NewsContentsModel> GetNewsAsync(int offset, int count)
        {
            var response = await GetAsync("news", offset, count);
            return response.ExJsonToEntity<NewsContentsModel>();
        }

        /// <summary>
        /// 获取图片(image), 视频(video), 语音(voice) 素材列表
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static MediumContentsModel GetMedium(string mediaType, int offset, int count)
        {
            var response = Get(mediaType, offset, count);
            return response.ExJsonToEntity<MediumContentsModel>();
        }

        /// <summary>
        /// 异步获取图片(image), 视频(video), 语音(voice) 素材列表
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async static Task<MediumContentsModel> GetMediumAsync(string mediaType, int offset, int count)
        {
            var response = await GetAsync(mediaType, offset, count);
            return response.ExJsonToEntity<MediumContentsModel>();
        }

    }
}

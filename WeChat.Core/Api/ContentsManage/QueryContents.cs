using System;
using System.Text;
using System.Threading.Tasks;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Api.ContentsManage.Models;
using WeChat.Core.Api.Models;

namespace WeChat.Core.Api.ContentsManage
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
            this.ReqData = Encoding.UTF8.GetBytes(new
            {
                type,
                offset,
                count
            }.ExToJson());
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
        public static QueryNewsContentsModel GetNews(int offset, int count)
        {
            var response = Get("news", offset, count);
            return response.ExJsonToEntity<QueryNewsContentsModel>();
        }

        /// <summary>
        /// 异步获取图文素材列表
        /// </summary>
        /// <param name="offset">从全部素材的该偏移位置开始返回 0表示从第一个素材返回</param>
        /// <param name="count">返回素材的数量, 取值在1到20之间</param>
        /// <returns></returns>
        public async static Task<QueryNewsContentsModel> GetNewsAsync(int offset, int count)
        {
            var response = await GetAsync("news", offset, count);
            return response.ExJsonToEntity<QueryNewsContentsModel>();
        }

        /// <summary>
        /// 获取素材列表
        /// </summary>
        /// <param name="mediaType">图片(image), 视频(video), 语音(voice) </param>
        /// <param name="offset">从全部素材的该偏移位置开始返回 0表示从第一个素材返回</param>
        /// <param name="count">返回素材的数量，取值在1到20之间</param>
        /// <returns></returns>
        public static MediumContentsModel GetMedium(string mediaType, int offset, int count)
        {
            var response = Get(mediaType, offset, count);
            return response.ExJsonToEntity<MediumContentsModel>();
        }

        /// <summary>
        /// 获取素材列表
        /// </summary>
        /// <param name="mediaType">图片(image), 视频(video), 语音(voice) </param>
        /// <param name="offset">从全部素材的该偏移位置开始返回 0表示从第一个素材返回</param>
        /// <param name="count">返回素材的数量，取值在1到20之间</param>
        /// <returns></returns>
        public async static Task<MediumContentsModel> GetMediumAsync(string mediaType, int offset, int count)
        {
            var response = await GetAsync(mediaType, offset, count);
            return response.ExJsonToEntity<MediumContentsModel>();
        }

    }
}

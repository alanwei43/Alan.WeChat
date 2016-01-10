using Alan.Utils.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.Core.Api.Models;
using WeChat.Core.Api.ContentsManage.Models;

namespace WeChat.Core.Api.ContentsManage
{
    /// <summary>
    /// 获取永久素材
    /// </summary>
    public class GetContent : ApiBase
    {
        protected async override Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (token.ErrCode != null && token.ErrCode != 0) throw new Exception(String.Format("下载图片时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/material/get_material?access_token={0}", token.Access_Token);

        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (token.ErrCode != null && token.ErrCode != 0) throw new Exception(String.Format("下载图片时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/material/get_material?access_token={0}", token.Access_Token);
        }


        /// <summary>
        /// 要获取的素材的media_id
        /// </summary>
        /// <param name="mediaId">素材Id</param>
        public GetContent(string mediaId)
        {
            this.ReqData = Encoding.UTF8.GetBytes(new { media_id = mediaId }.ExToJson());
        }

        /// <summary>
        /// 获取素材
        /// </summary>
        /// <returns></returns>
        private string Get()
        {
            var response = this.RequestAsString();
            return response;
        }

        /// <summary>
        /// 获取素材
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetAsync()
        {
            var response = await this.RequestAsStringAsync();
            return response;
        }

        /// <summary>
        /// 获取图文素材
        /// </summary>
        /// <param name="mediaId">素材Id</param>
        /// <returns></returns>
        public static GetNewsContentModel GetNews(string mediaId)
        {
            var get = new GetContent(mediaId);
            var response = get.Get();
            var model = response.ExJsonToEntity<GetNewsContentModel>();

            return model;
        }


        /// <summary>
        /// 获取图文素材
        /// </summary>
        /// <param name="mediaId">素材Id</param>
        /// <returns></returns>
        public static async Task<GetNewsContentModel> GetNewsAsync(string mediaId)
        {
            var get = new GetContent(mediaId);
            var response = await get.GetAsync();
            var model = response.ExJsonToEntity<GetNewsContentModel>();

            return model;
        }



        /// <summary>
        /// 获取图文素材
        /// </summary>
        /// <param name="mediaId">素材Id</param>
        /// <returns></returns>
        public static GetVideoContentModel GetVideo(string mediaId)
        {
            var get = new GetContent(mediaId);
            var response = get.Get();
            var model = response.ExJsonToEntity<GetVideoContentModel>();

            return model;
        }


        /// <summary>
        /// 获取图文素材
        /// </summary>
        /// <param name="mediaId">素材Id</param>
        /// <returns></returns>
        public static async Task<GetVideoContentModel> GetVideoAsync(string mediaId)
        {
            var get = new GetContent(mediaId);
            var response = await get.GetAsync();
            var model = response.ExJsonToEntity<GetVideoContentModel>();

            return model;
        }

    }
}

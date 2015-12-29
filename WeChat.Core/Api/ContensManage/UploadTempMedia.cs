using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.ContensManage
{
    /// <summary>
    /// 新增临时素材
    /// </summary>
    public class UploadTempMedia : ApiBase
    {
        private string _mediaType;
        protected override async Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            return String.Format("https://api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", token.Access_Token, this._mediaType);
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            return String.Format("https://api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", token.Access_Token, this._mediaType);
        }

        public UploadTempMedia() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediaType">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
        public UploadTempMedia(string mediaType) { }

        /// <summary>
        /// 新增临时素材
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public static UploadTempMedia Upload(string mediaType)
        {
            var upload = new UploadTempMedia(mediaType);
            upload
            return upload;
        }
    }
}

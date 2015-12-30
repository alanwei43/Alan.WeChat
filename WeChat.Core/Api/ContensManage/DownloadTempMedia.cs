using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.ContensManage
{
    /// <summary>
    /// 获取临时素材
    /// </summary>
    public class DownloadTempMedia : ApiBase
    {
        /// <summary>
        /// 媒体文件ID
        /// </summary>
        private string _mediaId;
        protected override async Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (!token.IsSuccess) throw new Exception(String.Format("获取临时素材时获取AccessToken错误: {0} {1}.", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", token.Access_Token, this._mediaId);

        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (!token.IsSuccess) throw new Exception(String.Format("获取临时素材时获取AccessToken错误: {0} {1}.", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", token.Access_Token, this._mediaId);
        }

        public DownloadTempMedia() { }

        public DownloadTempMedia(string mediaId)
        {
            this._mediaId = mediaId;
        }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }

        /// <summary>
        /// 下载媒体文件
        /// </summary>
        /// <param name="mediaId">媒体文件标识</param>
        /// <returns></returns>
        public static DownloadTempMedia Download(string mediaId)
        {
            var download = new DownloadTempMedia(mediaId);
            var disposition = "";
            var bytes = download.Request(fn =>
            {
                disposition = fn("Content-Disposition");
            });
            if (String.IsNullOrWhiteSpace(disposition) || disposition.IndexOf("filename=\"") == -1) return null;
            var fileNames = disposition.Replace("\"", "").Split('=');
            if (fileNames.Length != 2) return null;

            download.FileName = fileNames[1];
            download.FileData = bytes;
            return download;
        }

        /// <summary>
        /// 下载媒体文件
        /// </summary>
        /// <param name="mediaId">媒体文件标识</param>
        /// <returns></returns>
        public async static Task<DownloadTempMedia> DownloadAsync(string mediaId)
        {
            var download = new DownloadTempMedia(mediaId);
            var disposition = "";
            var bytes = await download.RequestAsync(fn =>
            {
                disposition = fn("Content-Disposition");
            });
            if (String.IsNullOrWhiteSpace(disposition) || disposition.IndexOf("filename=\"") == -1) return null;
            var fileNames = disposition.Replace("\"", "").Split('=');
            if (fileNames.Length != 2) return null;

            download.FileName = fileNames[1];
            download.FileData = bytes;
            return download;
        }
    }
}

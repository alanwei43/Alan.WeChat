using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.ContensManage
{
    public class DownloadMedia : ApiBase
    {
        private readonly string _mediaId;
        protected async override Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (token.ErrCode != null && token.ErrCode != 0) throw new Exception(String.Format("下载图片时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", token.Access_Token, this._mediaId);

        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (token.ErrCode != null && token.ErrCode != 0) throw new Exception(String.Format("下载图片时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", token.Access_Token, this._mediaId);
        }

        public string FileName { get; set; }
        public byte[] FileData { get; set; }

        public DownloadMedia() { }
        public DownloadMedia(string mediaId)
        {
            this._mediaId = mediaId;
        }

        /// <summary>
        /// 下载媒体文件
        /// </summary>
        /// <param name="mediaId">媒体文件标识</param>
        /// <returns></returns>
        public static DownloadMedia Download(string mediaId)
        {
            var download = new DownloadMedia(mediaId);
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
        public async static Task<DownloadMedia> DownloadAsync(string mediaId)
        {
            var download = new DownloadMedia(mediaId);
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

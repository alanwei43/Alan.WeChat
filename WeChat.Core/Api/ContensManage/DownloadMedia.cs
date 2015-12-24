using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.ContensManage
{
    public class DownloadMedia : ApiBase
    {
        private string mediaId;
        protected async override Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            return "https://zcapi.yupen.cn/Api/MobileProject/Recommand/1";
            if (token.ErrCode != null && token.ErrCode != 0) throw new Exception(String.Format("下载图片时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", token.Access_Token, this.mediaId);

        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (token.ErrCode != null && token.ErrCode != 0) throw new Exception(String.Format("下载图片时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", token.Access_Token, this.mediaId);
        }

        public string FileName { get; set; }
        public byte[] FileData { get; set; }

        public static DownloadMedia Download()
        {
            var download = new DownloadMedia();
            var disposition = "";
            var bytes = download.Request(fn =>
            {
                disposition = fn("Content-Disposition");
            });
            if (String.IsNullOrWhiteSpace(disposition) || disposition.IndexOf("filename=\"") == -1) return null;
            var fileNames = disposition.Replace("\"", "").Split('=');
            download.FileName = fileNames[1];
            download.FileData = bytes;
            return download;
        }

        public async static Task<DownloadMedia> DownloadAsync()
        {
            var download = new DownloadMedia();
            var disposition = "";
            var bytes = await download.RequestAsync(fn =>
            {
                disposition = fn("Content-Disposition");
            });
            if (String.IsNullOrWhiteSpace(disposition) || disposition.IndexOf("filename=\"") == -1) return null;
            var fileNames = disposition.Replace("\"", "").Split('=');
            download.FileName = disposition;
            download.FileData = bytes;
            return download;
        }
    }
}

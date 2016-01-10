using Alan.Utils.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.Core.Api.Models;

namespace WeChat.Core.Api.ContentsManage
{
    /// <summary>
    /// 上传图文消息内的图片
    /// </summary>
    public class UploadMedia : ApiBase
    {
        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (!token.IsSuccess) throw new Exception(String.Format("上传图文消息内的图片时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token={0}", token.Access_Token);
        }

        protected override async Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (!token.IsSuccess) throw new Exception(String.Format("上传图文消息内的图片时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token={0}", token.Access_Token);
        }
        protected override string ReqMethod
        {
            get
            {
                return "POST";
            }
        }
        /// <summary>
        /// 图片URL
        /// </summary>
        public string url { get; set; }

        public UploadMedia() { }
        public UploadMedia(byte[] fileData)
        {
            this.ReqData = fileData;
        }
        /// <summary>
        /// 组合获取ContentType
        /// </summary>
        /// <param name="mediaType">素材类型(iamge, voice, video, thumb)</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        private static string GetContentType(string mediaType, string fileName)
        {
            var fileType = mediaType == "thumb" ? "image" : mediaType;
            var extName = (System.IO.Path.GetExtension(fileName) ?? "").Replace(".", "").ToLower();
            if (String.IsNullOrWhiteSpace(extName)) throw new Exception("文件扩展名无效.");
            var contentType = String.Format("{0}/{1}", fileType, extName);
            return contentType;
        }


        /// <summary>
        /// 上传图片素材
        /// </summary>
        /// <param name="data">文件数据</param>
        /// <param name="mediaType">素材类型</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static UploadMedia Upload(byte[] data, string mediaType, string fileName)
        {
            var upload = new UploadMedia(data);
            var response = upload.UploadFile(fileName, GetContentType(mediaType, fileName));
            var responseText = Encoding.UTF8.GetString(response);
            return responseText.ExJsonToEntity<UploadMedia>();
        }


        /// <summary>
        /// 上传图片素材
        /// </summary>
        /// <param name="data">文件数据</param>
        /// <param name="mediaType">素材类型</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static async Task<UploadMedia> UploadAsync(byte[] data, string mediaType, string fileName)
        {
            var upload = new UploadMedia(data);
            var response = await upload.UploadFileAsync(fileName, GetContentType(mediaType, fileName));
            var responseText = Encoding.UTF8.GetString(response);
            return responseText.ExJsonToEntity<UploadMedia>();
        }




    }
}

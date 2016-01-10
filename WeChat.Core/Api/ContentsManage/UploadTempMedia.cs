using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Utils;
using WeChat.Core.Api.Models;

namespace WeChat.Core.Api.ContentsManage
{
    /// <summary>
    /// 新增临时素材(上传多媒体文件)
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
        /// 新增临时素材
        /// </summary>
        /// <param name="mediaType">媒体文件类型, 分别有图片(image), 语音(voice), 视频(video)和缩略图(thumb) </param>
        public UploadTempMedia(byte[] data, string mediaType)
        {
            this.ReqData = data;
            this._mediaType = mediaType;
        }
        public UploadTempMedia(string fileFullPath, string mediaType)
        {
            this.ReqData = File.ReadAllBytes(fileFullPath);
            this._mediaType = mediaType;
        }
        /// <summary>
        /// 媒体文件上传后, 获取时的唯一标识
        /// </summary>
        public string Media_Id { get; set; }

        /// <summary>
        /// 媒体文件类型 分别有图片(image), 语音(voice), 视频(video)和缩略图(thumb, 主要用于视频与音乐格式的缩略图)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 媒体文件上传时间戳
        /// </summary>
        public long Created_At { get; set; }

        /// <summary>
        /// 组合获取ContentType
        /// </summary>
        /// <param name="mediaType">素材类型(iamge, voice, video, thumb)</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        private static string GetContentType(string mediaType, string fileName)
        {
            var fileType = mediaType == "thumb" ? "image" : mediaType;
            var extName = (Path.GetExtension(fileName) ?? "").Replace(".", "").ToLower();
            if (String.IsNullOrWhiteSpace(extName)) throw new Exception("文件扩展名无效.");
            var contentType = String.Format("{0}/{1}", fileType, extName);
            return contentType;
        }

        /// <summary>
        /// 新增临时素材
        /// </summary>
        /// <param name="data">需要上传的数据</param>
        /// <param name="mediaType">素材类型, 分别有图片(image), 语音(voice), 视频(video)和缩略图(thumb)</param>
        /// <param name="fileName">文件名(一定要包括正确的扩展名, 比如 hello.jpg)</param>
        /// <returns></returns>
        public static UploadTempMedia Upload(byte[] data, string mediaType, string fileName)
        {
            var contentType = GetContentType(mediaType, fileName);
            var upload = new UploadTempMedia(data, mediaType);
            var response = upload.UploadFile(fileName, contentType);
            return Encoding.UTF8.GetString(response).ExJsonToEntity<UploadTempMedia>();
        }

        /// <summary>
        /// 新增临时素材
        /// </summary>
        /// <param name="fileFullPath">文件的绝对路径</param>
        /// <param name="mediaType">媒体文件类型, 分别有图片(image), 语音(voice), 视频(video)和缩略图(thumb)</param>
        /// <returns></returns>
        public static UploadTempMedia Upload(string fileFullPath, string mediaType)
        {
            var fileBytes = File.ReadAllBytes(fileFullPath);
            var fileName = Path.GetFileName(fileFullPath);
            return Upload(fileBytes, mediaType, fileName);
        }

        /// <summary>
        /// 新增临时素材
        /// </summary>
        /// <param name="fileRelativePath">文件的相对路径</param>
        /// <param name="mediaType">媒体文件类型, 分别有图片(image), 语音(voice), 视频(video)和缩略图(thumb)</param>
        /// <returns></returns>
        public static UploadTempMedia UploadWithPath(string fileRelativePath, string mediaType)
        {
            var fileFullPath = System.Web.Hosting.HostingEnvironment.MapPath(fileRelativePath);
            return Upload(fileFullPath, mediaType);
        }




        /// <summary>
        /// 新增临时素材
        /// </summary>
        /// <param name="data">需要上传的数据</param>
        /// <param name="mediaType">素材类型, 分别有图片(image), 语音(voice), 视频(video)和缩略图(thumb)</param>
        /// <param name="fileName">文件名(一定要包括正确的扩展名, 比如 hello.jpg)</param>
        /// <returns></returns>
        public static async Task<UploadTempMedia> UploadAsync(byte[] data, string mediaType, string fileName)
        {
            var contentType = GetContentType(mediaType, fileName);
            var upload = new UploadTempMedia(data, mediaType);
            var response = await upload.UploadFileAsync(fileName, contentType);
            return Encoding.UTF8.GetString(response).ExJsonToEntity<UploadTempMedia>();
        }

        /// <summary>
        /// 新增临时素材
        /// </summary>
        /// <param name="fileFullPath">文件的绝对路径</param>
        /// <param name="mediaType">媒体文件类型, 分别有图片(image), 语音(voice), 视频(video)和缩略图(thumb)</param>
        /// <returns></returns>
        public static async Task<UploadTempMedia> UploadAsync(string fileFullPath, string mediaType)
        {
            var fileBytes = File.ReadAllBytes(fileFullPath);
            var fileName = Path.GetFileName(fileFullPath);
            return await UploadAsync(fileBytes, mediaType, fileName);
        }

        /// <summary>
        /// 新增临时素材
        /// </summary>
        /// <param name="fileRelativePath">文件的相对路径</param>
        /// <param name="mediaType">媒体文件类型, 分别有图片(image), 语音(voice), 视频(video)和缩略图(thumb)</param>
        /// <returns></returns>
        public static async Task<UploadTempMedia> UploadWithPathAsync(string fileRelativePath, string mediaType)
        {
            var fileFullPath = System.Web.Hosting.HostingEnvironment.MapPath(fileRelativePath);
            return await UploadAsync(fileFullPath, mediaType);
        }




    }
}

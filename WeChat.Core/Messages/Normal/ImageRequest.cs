using System.Xml.Serialization;

namespace WeChat.Core.Messages.Normal
{
    /// <summary>
    /// 图片消息
    /// </summary>
    [XmlRoot("xml")]
    public class ImageRequest : NormalBase
    {
        /// <summary>
        /// 图片链接
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 图片消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { get; set; }


    }
}

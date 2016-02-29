using System.Xml.Linq;
using WeChat.Core.Utils;

namespace WeChat.Core.Messages.Normal
{
    /// <summary>
    /// 回复图片消息
    /// </summary>
    public class ImageResponse : RequestBase
    {
        public ImageResponse()
        {
            this.MsgType = Configurations.Current.MessageType.Image;
        }

        /// <summary>
        /// 图片资源
        /// </summary>
        public ImageModel Image { get; set; }

        public class ImageModel
        {
            /// <summary>
            /// 通过上传多媒体文件，得到的id
            /// </summary>
            public string MediaId { get; set; }
        }

        public override string ToXml()
        {
            if (this.Image == null) this.Image = new ImageModel();

            XElement ele = new XElement("xml",
                ModelBase.ToXmls(this),
                new XElement("Image", ToXmls<ImageModel>(this.Image)));

            return ele.ToString();
        }
    }
}

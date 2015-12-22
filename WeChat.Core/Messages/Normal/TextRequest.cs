using System.Xml.Serialization;

namespace WeChat.Core.Messages.Normal
{
    /// <summary>
    /// 文本消息
    /// </summary>
    [XmlRoot("xml")]
    public class TextRequest : NormalBase
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }

    }
}

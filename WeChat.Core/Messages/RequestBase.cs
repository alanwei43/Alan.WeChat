using System.Xml.Serialization;

namespace WeChat.Core.Messages
{
    /// <summary>
    /// 基础请求模型
    /// </summary>
    [XmlRoot("xml")]
    public class RequestBase : ModelBase
    {
     
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// 随机数
        /// </summary>
        public string Nonce { get; set; }
    }
}

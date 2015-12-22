using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace WeChat.Core.Messages
{
    /// <summary>
    /// 基础模型
    /// </summary>
    [XmlRoot("xml")]
    public class ModelBase
    {
        /// <summary>
        /// Request: 开发者微信号
        /// Response: 接收方帐号（收到的OpenID）
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// Request: 发送方帐号（一个OpenID）
        /// Reponse: 开发者微信号
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get; set; }


        public virtual string ToXml()
        {
            var xele = new XElement("xml", ToXmls(this));
            return xele.ToString();
        }

        public static IEnumerable<XElement> ToXmls<T>(T item)
        {
            return item.GetType().GetProperties().Select(property => new XElement(property.Name, property.GetValue(item)));
        }
    }
}

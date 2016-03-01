using System.Xml.Serialization;

namespace WeChat.Core.Messages
{
    /// <summary>
    /// 接收事件推送 
    /// </summary>
    [XmlRoot("xml")]
    public class EventBase : RequestBase
    {
        /// <summary>
        /// 事件类型 subscribe(订阅), unsubscribe(取消订阅)
        /// </summary>
        public string Event { get; set; }

    }
}

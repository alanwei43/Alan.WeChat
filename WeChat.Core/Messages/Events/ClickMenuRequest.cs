using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WeChat.Core.Messages.Events
{

    /// <summary>
    /// 点击菜单拉取消息时的事件推送, Event: CLICK
    /// 点击菜单跳转链接时的事件推送, Event: VIEW
    /// </summary>
    [XmlRoot("xml")]
    public class ClickMenuRequest : EventBase
    {
        public string EventKey { get; set; }
    }
}

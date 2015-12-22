using System.Collections.Generic;
using System.Linq;

namespace WeChat.Core.Messages
{
    public class MessageTypes
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        public string Text { get { return "text"; } }

        /// <summary>
        /// 图片消息
        /// </summary>
        public string Image { get { return "image"; } }

        /// <summary>
        /// 语音消息
        /// </summary>
        public string Voice { get { return "voice"; } }

        /// <summary>
        /// 地理位置消息
        /// </summary>
        public string Location { get { return "location"; } }

        /// <summary>
        /// 链接消息
        /// </summary>
        public string Link { get { return "link"; } }

        /// <summary>
        /// 回复图文消息
        /// </summary>
        public string News { get { return "news"; } }

        /// <summary>
        /// 事件消息
        /// </summary>
        public string Event { get { return "event"; } }

        public List<string> AllTypes()
        {
            return this.GetType().GetProperties().Select(property => property.GetValue(this).ToString()).ToList();
        }
        public bool Contains(string messageTypeName)
        {
            return AllTypes().Contains(messageTypeName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Messages.Events
{
    public class EventTypes
    {
        /// <summary>
        /// 关注事件
        /// </summary>
        public string Subscribe { get { return "subscribe"; } }

        /// <summary>
        /// 取消关注
        /// </summary>
        public string UnSubscribe { get { return "unsubscribe"; } }

        /// <summary>
        /// 扫描带参数二维码事件 用户已关注时的事件推送
        /// </summary>
        public string Scan { get { return "SCAN"; } }

        /// <summary>
        /// 上报地理位置事件
        /// </summary>
        public string Location { get { return "LOCATION"; } }

        /// <summary>
        /// 点击菜单拉取消息时的事件推送
        /// </summary>
        public string Click { get { return "CLICK"; } }

        /// <summary>
        /// 点击菜单跳转链接时的事件推送
        /// </summary>
        public string View { get { return "VIEW"; } }

        public string PickSysPhoto { get { return "pic_sysphoto"; } }


    }
}

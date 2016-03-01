using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WeChat.Core.Messages.Events
{
    [XmlRoot("xml")]
    public class PickPhotoRequest : EventBase
    {
        /// <summary>
        /// 事件KEY值，由开发者在创建菜单时设定
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// 发送的图片信息
        /// </summary>
        public SendPicsInfoModel SendPicsInfo { get; set; }

    }

    /// <summary>
    /// 发送的图片信息
    /// </summary>
    public class SendPicsInfoModel
    {
        /// <summary>
        /// 发送的图片数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 图片列表
        /// </summary>
        public List<PicListModel> PicList { get; set; }


        public class PicListModel
        {
            public ItemModel item { get; set; }
            public class ItemModel
            {
                /// <summary>
                /// 图片的MD5值，开发者若需要，可用于验证接收到图片
                /// </summary>
                public string PicMd5Sum { get; set; }
            }
        }
    }


}

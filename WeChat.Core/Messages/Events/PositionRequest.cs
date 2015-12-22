using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WeChat.Core.Messages.Events
{
    /// <summary>
    /// 上报地理位置事件
    /// </summary>
    [XmlRoot("xml")]
    public class PositionRequest : EventBase
    {
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// 地理位置精度
        /// </summary>
        public float Precision { get; set; }
    }
}

using System.Xml.Serialization;

namespace WeChat.Core.Messages.Normal
{
    [XmlRoot("xml")]
    public class LocationRequest : NormalBase
    {
        /// <summary>
        /// 地理位置维度
        /// </summary>
        public string Location_X { get; set; }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y { get; set; }

        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public int Scale { get; set; }

        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WeChat.Core.Messages.Events
{
    /// <summary>
    /// 接收语音识别结果
    /// </summary>
    [XmlRoot("xml")]
    public class VoiceRequest : EventBase
    {
        public VoiceRequest()
        {
            this.Event = "VOICE";
        }


        /// <summary>
        /// 消息id, 64位整型
        /// </summary>
        public string MsgID { get; set; }

        /// <summary>
        /// 语音格式: amr
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 语音消息媒体id, 可以调用多媒体文件下载接口拉取该媒体
        /// </summary>
        public string MediaID { get; set; }

        /// <summary>
        /// 语音识别结果, UTF8编码
        /// </summary>
        public string Recognition { get; set; }
    }
}

using System;
using System.IO;
using System.Net;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Messages;
using WeChat.Core.Messages.Events;
using WeChat.Core.Messages.Normal;

namespace WeChat.Core.Utils
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class Configurations
    {

        #region 单例

        public static Configurations Current { get; private set; }

        static Configurations()
        {
            Current = new Configurations();
        }

        public static void Inject(string configJson)
        {
            Inject(configJson.ExJsonToEntity<Configurations>());
        }

        public static void Inject(Configurations config)
        {
            Current = config;
        }
        #endregion


        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public long GetTimeStamp()
        {
            var timeStamp = new DateTime(1970, 1, 1);
            return (long)(DateTime.Now - timeStamp).TotalSeconds;
        }

        /// <summary>
        /// Hash校验Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 对称加密密钥
        /// </summary>
        public string AesKey { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 微信 应用密钥
        /// </summary>
        public string AppSecret { get; set; }

        private string _messageMode;

        /// <summary>
        /// 消息模式
        /// Plain 明文模式
        /// Hybrid 兼容模式
        /// Cipher 安全模式
        /// </summary>
        public string MessageMode
        {
            get { return this._messageMode; }
            set
            {
                this._messageMode = value;
                if (!String.IsNullOrWhiteSpace(this._messageMode))
                    this._messageMode = TransferMode.Plain.ToString().ToLower();

                this.EnumMessageMode = TransferMode.Plain;

                if (this._messageMode.ToLower() == Configurations.TransferMode.Cipher.ToString().ToLower())
                {
                    this.EnumMessageMode = TransferMode.Cipher;
                }
                if (this._messageMode.ToLower() == Configurations.TransferMode.Hybrid.ToString().ToLower())
                {
                    this.EnumMessageMode = TransferMode.Hybrid;
                }
            }
        }

        /// <summary>
        /// 消息模式
        /// 1: 明文模式
        /// 2: 兼容模式
        /// 3: 安全模式
        /// </summary>
        public TransferMode EnumMessageMode { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageTypes MessageType
        {
            get { return new MessageTypes(); }
        }


        /// <summary>
        /// 事件类型
        /// </summary>
        public EventTypes EventType { get { return new EventTypes(); } }



        /// <summary>
        /// 数据传输模式
        /// </summary>
        public enum TransferMode : byte
        {
            /// <summary>
            /// 明文模式
            /// </summary>
            Plain = 0,

            /// <summary>
            /// 混合模式
            /// </summary>
            Hybrid = 1,

            /// <summary>
            /// 加密模式
            /// </summary>
            Cipher = 2
        }

    }
}

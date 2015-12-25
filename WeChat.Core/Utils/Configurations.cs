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

        public static void InjectWithFile(string fileFullPath)
        {
            Current = File.ReadAllText(fileFullPath).ExJsonToEntity<Configurations>();
        }
        public static void Inject(string configJson)
        {
            Current = configJson.ExJsonToEntity<Configurations>();
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

    }
}

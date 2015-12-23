using System;
using System.IO;
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
        protected static string ConfigPath
        {
            get
            {
                var configPath = System.Web.Configuration.WebConfigurationManager.AppSettings["/Configuration/Path"];
                if (String.IsNullOrWhiteSpace(configPath)) configPath = "~/App_Data/Config.json";

                if (configPath.StartsWith("~"))
                    return System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Config.json");
                return configPath;
            }
        }

        public static Configurations Current { get; private set; }

        static Configurations()
        {
            if (File.Exists(ConfigPath))
            {
                Current = File.ReadAllText(ConfigPath).ExJsonToEntity<Configurations>();
            }
            else throw new FileNotFoundException(ConfigPath);
        }

        #endregion

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

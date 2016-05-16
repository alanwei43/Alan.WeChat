using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Alan.Utils;
using Alan.Utils.UtililyMethods;
using Alan.Utils.ExtensionMethods;

namespace Alan.WeChat.CleverTangYuan.Library.WeChat
{

    public sealed class Config : Configurations<Config>, IConfigurationModel
    {
     
        public string Token { get; set; }
        public string AESKey { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }

        public string ConfigPath
        {
            get
            {
                return System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/WeChat/config.json");
            }

        }
    }
}
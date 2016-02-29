using Alan.Utils.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WeChat.Core.Utils;

namespace WeChat.Example.Library
{
    public class MyConfig : Configurations
    {
        public static MyConfig Current { get; set; }

        static MyConfig()
        {
            var filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Config.json");
            Current = File.ReadAllText(filePath).ExJsonToEntity<MyConfig>();
            Configurations.Inject(Current);
        }
        public string SqlConnection { get; set; }
        public string BaiduApiKey { get; set; }
    }
}
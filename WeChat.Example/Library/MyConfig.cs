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
            Current = File.ReadAllText(Configurations.ConfigPath).ExJsonToEntity<MyConfig>();
        }
        public string SqlConnection { get; set; }
    }
}
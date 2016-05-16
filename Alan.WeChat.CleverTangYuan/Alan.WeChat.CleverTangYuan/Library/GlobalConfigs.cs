using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alan.WeChat.CleverTangYuan.Library
{
    public class GlobalConfigs : Configurations<GlobalConfigs>, IConfigurationModel
    {
        public string AliHostSql { get; set; }
        public string ConfigPath
        {
            get
            {
                return System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/global.json");
            }
        }
    }
}
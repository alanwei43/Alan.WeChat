using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alan.WeChat.CleverTangYuan.Library
{
    public interface IConfigurationModel
    {
        string ConfigPath { get; }
    }
    public abstract class Configurations<T>
        where T : IConfigurationModel, new()
    {
        private static T current;
        public static T Current
        {
            get
            {
                if (current == null)
                {
                    var instance = new T();
                    var configJson = System.IO.File.ReadAllText(instance.ConfigPath);
                    var model = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(configJson);
                    current = model;
                }
                return current;

            }
        }
    }
}
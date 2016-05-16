using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alan.WeChat.CleverTangYuan.Library
{

    /// <summary>
    /// 单利模式
    /// via: https://msdn.microsoft.com/en-us/library/ff650316.aspx
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T>
        where T : new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;

            }
        }


    }
}
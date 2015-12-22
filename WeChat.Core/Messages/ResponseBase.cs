using System;

namespace WeChat.Core.Messages
{
    public class ResponseBase : ModelBase
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public new string CreateTime { get { return (DateTime.Now - (new DateTime(1970, 1, 1))).Ticks.ToString(); } }

    }
}

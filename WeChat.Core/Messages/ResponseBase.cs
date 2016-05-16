using System;

namespace WeChat.Core.Messages
{
    public class ResponseBase : ModelBase
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public new string CreateTime { get { return (DateTime.Now - (new DateTime(1970, 1, 1))).Ticks.ToString(); } }

        /// <summary>
        /// 实例化一个空响应
        /// </summary>
        /// <returns></returns>
        public static NullResponseBase GetNullResponseModel()
        {
            return new NullResponseBase();
        }

        /// <summary>
        /// 当前是否是 NullResponseModel实例
        /// </summary>
        /// <returns></returns>
        public bool IsNullResponseModel()
        {
            return this is NullResponseBase;
        }
    }
}

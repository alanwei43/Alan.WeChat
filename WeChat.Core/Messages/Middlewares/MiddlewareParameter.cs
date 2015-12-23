using System.Collections.Generic;

namespace WeChat.Core.Messages
{
    /// <summary>
    /// 中间件输入
    /// </summary>
    public class MiddlewareParameter
    {
        public MiddlewareParameter()
        {
            this.Items = new Dictionary<string, object>();
            this.Output = new MiddlewareOutput { ResponseModel = new ResponseBase() };
        }

        /// <summary>
        /// 辅助用于传递数据
        /// </summary>
        public Dictionary<string, object> Items { get; set; }

        /// <summary>
        /// 输入
        /// </summary>
        public MiddlewareInput Input { get; set; }

        /// <summary>
        /// 输出
        /// </summary>
        private MiddlewareOutput Output { get; set; }

        /// <summary>
        /// 设置输出结果
        /// </summary>
        /// <param name="rep"></param>
        public void SetResponseModel(ResponseBase rep)
        {
            this.Output.ResponseModel = rep;
        }


        public string GetResponse()
        {
            return this.Output.Response;
        }
    }
}

namespace WeChat.Core.Messages
{
    /// <summary>
    /// 中间件输入
    /// </summary>
    public class MiddlewareParameter
    {

        /// <summary>
        /// 输入
        /// </summary>
        public MiddlewareInput Input { get; set; }

        /// <summary>
        /// 输出
        /// </summary>
        public MiddlewareOutput Output { get; set; }

        public string GetResponse()
        {
            return this.Output.Response;
        }
    }
}

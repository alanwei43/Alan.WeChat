namespace WeChat.Core.Api.JsSdk.Models
{
    /// <summary>
    /// 微信JS SDK配置
    /// </summary>
    public class JsSdkConfigModel
    {
        /// <summary>
        /// App Id
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string RandomString { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }

        public object Other { get; set; }

    }

}

using Alan.Utils.ExtensionMethods;
using System.Collections.Generic;

namespace WeChat.Core.Messages.Middlewares
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

            this.Items.Add("/Sys/SetResponse", false);  //是否已经设置了Response
            this.Items.Add("/Sys/SetResponseCount", 0); //设置Response的次数
        }

        public object GetItem(string key)
        {
            if (!this.Items.ContainsKey(key)) return null;
            var value = this.Items[key];
            return value;
        }
        public T GetItem<T>(string key)
        {
            if (!this.Items.ContainsKey(key)) return default(T);
            var value = this.Items[key];
            return value.ExChangeType<T>();
        }
        public void SetItem(string key, object value)
        {
            this.Items[key] = value;
        }
        /// <summary>
        /// 辅助用于传递数据
        /// </summary>
        public Dictionary<string, object> Items { get; private set; }

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
            this.SetItem("/Sys/SetResponse", true);
            this.SetItem("/Sys/SetResponseItemCount", (this.GetItem("/Sys/SetResponseCount") as int?).GetValueOrDefault() + 1);

            this.Output.ResponseModel = rep;
        }

        /// <summary>
        /// 获取输出内容
        /// </summary>
        /// <returns></returns>
        public string GetResponse()
        {
            return this.Output.Response;
        }
    }
}

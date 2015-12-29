using System;
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
            this.Output = new MiddlewareOutput();

            this.Items.Add("/Sys/SetResponse", false);  //是否已经设置了Response
            this.Items.Add("/Sys/SetResponseCount", 0); //设置Response的次数
        }
        public bool SetedResponse { get { return this.GetItem<bool>("/Sys/SetResponse"); } }

        /// <summary>
        /// 获取条目
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetItem(string key)
        {
            if (!this.Items.ContainsKey(key)) return null;
            var value = this.Items[key];
            return value;
        }

        /// <summary>
        /// 获取自定义数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetItem<T>(string key)
            where T : struct
        {
            return (this.GetItem(key) as T?).GetValueOrDefault();
        }

        /// <summary>
        /// 设置自定义数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetItem(string key, object value)
        {
            this.Items[key] = value;
        }

        /// <summary>
        /// 辅助用于传递数据
        /// </summary>
        private Dictionary<string, object> Items { get; set; }

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
            this.SetItem("/Sys/SetResponseItemCount", this.GetItem<int>("/Sys/SetResponseCount") + 1);

            this.Output.ResponseModel = rep;
        }

        /// <summary>
        /// 获取输出内容
        /// </summary>
        /// <returns></returns>
        public string GetResponse()
        {
            if (this.Output.ResponseModel != null)
            {
                this.Output.ResponseModel.ToUserName = this.Input.RequestBaseModel.FromUserName;
                this.Output.ResponseModel.FromUserName = this.Input.RequestBaseModel.ToUserName;
            }
            return this.Output.Response;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Alan.Utils.ExtensionMethods;

namespace WeChat.Core.Api
{
    public abstract class ApiBase
    {
        /// <summary>
        /// 请求的URL
        /// </summary>
        protected abstract Task<string> GetApiUrlAsync();

        protected abstract string GetApiUrl();

        /// <summary>
        /// 请求的数据
        /// </summary>
        protected virtual string ReqData { get; set; }

        /// <summary>
        /// 请求的方法
        /// </summary>
        protected virtual string ReqMethod { get { return "GET"; } }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <returns></returns>
        protected async Task<string> SendRequestAsync()
        {
            HttpClient client = new HttpClient();
            string response;

            var apiUrl = await this.GetApiUrlAsync();

            if (this.ReqMethod.ToUpper() == "POST")
            {
                var rep = await client.PostAsync(apiUrl, new StringContent(this.ReqData, Encoding.UTF8));
                response = await rep.Content.ReadAsStringAsync();
            }
            else response = await client.GetStringAsync(apiUrl);

            return response;
        }

        protected async Task<T> SendRequestAsync<T>()
            where T : class
        {
            var response = await this.SendRequestAsync();
            return response.ExJsonToEntity<T>();
        }

        protected string SendRequest()
        {
            WebRequest req = WebRequest.Create(this.GetApiUrl());

            if (this.ReqMethod.ToUpper() == "POST")
            {
                req.Method = "POST";
                using (StreamWriter writer = new StreamWriter(req.GetRequestStream()))
                    writer.Write(this.ReqData);
            }

            using (var rep = req.GetResponse())
            {
                using (StreamReader reader = new StreamReader(rep.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }

        }

        protected T SendRequest<T>()
            where T : class
        {
            var rep = this.SendRequest();
            return rep.ExJsonToEntity<T>();
        }


        /// <summary>
        /// 错误码
        /// </summary>
        public long? ErrCode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}

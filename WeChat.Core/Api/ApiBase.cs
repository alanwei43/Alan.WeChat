using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Log;
using WeChat.Core.Utils;

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
        protected virtual byte[] ReqData { get; set; }

        protected string ReqDataString { get { return Encoding.UTF8.GetString(this.ReqData); } }
        /// <summary>
        /// 请求的方法
        /// </summary>
        protected virtual string ReqMethod { get { return "GET"; } }

        public ApiBase() { }


        protected T RequestAsModel<T>()
            where T : class
        {
            return this.RequestAsString().ExJsonToEntity<T>();
        }

        protected async Task<T> RequestAsModelAsync<T>()
            where T : class
        {
            var response = await this.RequestAsStringAsync();
            return response.ExJsonToEntity<T>();
        }

        protected string RequestAsString()
        {
            var bytes = this.Request(null);
            return Encoding.UTF8.GetString(bytes);
        }

        protected async Task<string> RequestAsStringAsync()
        {
            var bytes = await this.RequestAsync(null);
            return Encoding.UTF8.GetString(bytes);
        }

        protected byte[] Request(Action<Func<string, string>> getHeaders)
        {
            WebRequest req = WebRequest.Create(this.GetApiUrl());

            if (this.ReqMethod.ToUpper() == "POST")
            {
                req.Method = "POST";
                using (StreamWriter writer = new StreamWriter(req.GetRequestStream()))
                    writer.Write(this.ReqData);
            }

            List<byte> buffers = new List<byte>();
            using (var rep = req.GetResponse())
            {

                using (Stream reader = rep.GetResponseStream())
                {
                    if (reader == null) return null;
                    byte[] buffer = new byte[1024];
                    int readLength = 0;
                    do
                    {
                        readLength = reader.Read(buffer, 0, buffer.Length);
                        buffers.AddRange(buffer.Take(readLength));
                    } while (readLength > 0);
                }

                if (getHeaders != null)
                {
                    getHeaders((key) => rep.Headers[key]);
                }
            }

            return buffers.ToArray();
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <returns></returns>
        protected async Task<byte[]> RequestAsync(Action<Func<string, string>> getHeaders)
        {

            HttpClient client = new HttpClient();
            byte[] response;

            var apiUrl = await this.GetApiUrlAsync();

            HttpResponseMessage rep;

            if (this.ReqMethod.ToUpper() == "POST")
                rep = await client.PostAsync(apiUrl, new StringContent(this.ReqDataString, Encoding.UTF8));
            else
                rep = await client.GetAsync(apiUrl);

            response = await rep.Content.ReadAsByteArrayAsync();

            if (getHeaders != null)
            {

                getHeaders(key => String.Join("", rep.Content.Headers.GetValues(key)));
            }
            return response;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="contentType">文件的 Content-Type</param>
        /// <returns></returns>
        protected byte[] UploadFile(string fileName, string contentType)
        {
            var url = this.GetApiUrl();
            var response = HttpUtils.UploadFile(url, new HttpUtils.FormFileParam(fileName, System.IO.Path.GetFileNameWithoutExtension(fileName), this.ReqData), contentType, null);
            return response;
        }



        /// <summary>
        /// 错误码
        /// </summary>
        public long? ErrCode { get; set; }
        /// <summary>
        /// 接口是否调用成功
        /// </summary>
        public bool IsSuccess { get { return this.ErrCode.GetValueOrDefault() == 0; } }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}

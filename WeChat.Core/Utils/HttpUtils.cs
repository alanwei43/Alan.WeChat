using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Alan.Utils.ExtensionMethods;

namespace WeChat.Core.Utils
{
    public class HttpUtils
    {
        private string Url { get; set; }
        private string Data { get; set; }
        private string HttpMethod { get; set; }

        public HttpUtils(string url, string data, string method)
        {
            this.Url = url;
            this.Data = data;
            this.HttpMethod = method ?? "GET";
        }

        public static HttpUtils Get(string url, string data, string method)
        {
            return new HttpUtils(url, data, method);
        }

        public T RequestAsModel<T>()
            where T : class
        {
            var rep = this.RequestAsString();
            return rep.ExJsonToEntity<T>();
        }

        public async Task<T> RequestAsModelAsync<T>()
            where T : class
        {
            var response = await this.RequestAsStringAsync();
            return response.ExJsonToEntity<T>();
        }

        public string RequestAsString()
        {
            var bytes = this.Request(null);
            return Encoding.UTF8.GetString(bytes);
        }

        public async Task<string> RequestAsStringAsync()
        {
            var bytes = await this.RequestAsync(null);
            return Encoding.UTF8.GetString(bytes);
        }

        public byte[] Request(Action<Func<string, string>> getHeaders)
        {
            WebRequest req = WebRequest.Create(this.Url);

            if (this.HttpMethod.ToUpper() == "POST")
            {
                req.Method = "POST";
                using (StreamWriter writer = new StreamWriter(req.GetRequestStream()))
                    writer.Write(this.Data);
            }

            List<byte> buffers = new List<byte>();
            using (var rep = req.GetResponse())
            {
                using (Stream reader = rep.GetResponseStream())
                {
                    if (reader == null) return null;
                    byte[] buffer = new byte[1024];
                    int readLength;
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

        public async Task<byte[]> RequestAsync(Action<Func<string, string>> getHeaders)
        {

            HttpClient client = new HttpClient();
            byte[] response;


            HttpResponseMessage rep;

            if (this.HttpMethod.ToUpper() == "POST")
                rep = await client.PostAsync(this.Url, new StringContent(this.Data, Encoding.UTF8));
            else
                rep = await client.GetAsync(this.Url);

            response = await rep.Content.ReadAsByteArrayAsync();

            if (getHeaders != null)
            {

                getHeaders(key => String.Join("", rep.Content.Headers.GetValues(key)));
            }
            return response;
        }

        public System.Tuple<string, byte[]> DownloadFile()
        {
            var fileName = String.Empty;
            var bytes = this.Request(getHeader =>
            {
                var disposition = getHeader("Content-Disposition");
                if (String.IsNullOrWhiteSpace(disposition))
                {
                    var contentType = (getHeader("Content-Type") ?? "").Split('/');
                    if (contentType.Length > 1) fileName = String.Format("{0}.{1}", Guid.NewGuid(), contentType[1]);
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(disposition) || disposition.IndexOf("filename=\"") == -1) return;
                    var fileNames = disposition.Replace("\"", "").Split('=');
                    if (fileNames.Length != 2) return;
                    fileName = fileNames[1];
                }
            });
            if (String.IsNullOrWhiteSpace(fileName)) return Tuple.Create(Guid.NewGuid().ToString(), bytes);
            return System.Tuple.Create(fileName, bytes);
        }

        public async Task<Tuple<string, byte[]>> DownloadFileAsync()
        {
            var fileName = String.Empty;
            var bytes = await this.RequestAsync(getHeader =>
            {
                var disposition = getHeader("Content-Disposition");
                if (String.IsNullOrWhiteSpace(disposition))
                {
                    var contentType = (getHeader("Content-Type") ?? "").Split('/');
                    if (contentType.Length > 1) fileName = String.Format("{0}.{1}", Guid.NewGuid(), contentType[1]);
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(disposition) || disposition.IndexOf("filename=\"") == -1) return;
                    var fileNames = disposition.Replace("\"", "").Split('=');
                    if (fileNames.Length != 2) return;
                    fileName = fileNames[1];
                }
            });
            if (String.IsNullOrWhiteSpace(fileName)) return Tuple.Create(Guid.NewGuid().ToString(), bytes);
            return System.Tuple.Create(fileName, bytes);
        }

        public string UploadFile()
        {
            WebClient client = new WebClient();
            //client.UploadFile(this.Url, )
            return null;
        }
    }
}

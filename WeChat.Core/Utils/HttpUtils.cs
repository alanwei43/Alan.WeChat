using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
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


        #region 上传文件
        /// <summary>
        /// 表单上传文件信息
        /// </summary>
        public class FormFileParam
        {
            /// <summary>
            /// 文件数据
            /// </summary>
            public byte[] Data { get; set; }

            /// <summary>
            /// 文件名
            /// </summary>
            public string FileName { get; set; }

            /// <summary>
            /// 文件对应的Form参数名
            /// </summary>
            public string ParamName { get; set; }

            /// <summary>
            /// 文件的ContentType
            /// </summary>
            public string ContentType { get; set; }

            /// <summary>
            /// 构造Form表单单个文件数据
            /// </summary>
            /// <param name="fileName">文件名</param>
            /// <param name="paramName">文件参数名</param>
            /// <param name="data">文件数据</param>
            /// <param name="contentType">文件的ContentType</param>
            public FormFileParam(string fileName, string paramName, byte[] data, string contentType)
            {
                this.FileName = fileName;
                this.ParamName = paramName;
                this.Data = data;
                this.ContentType = contentType;
            }

        }

        /// <summary>
        /// 以Form表单的形式上传文件
        /// </summary>
        /// <param name="url">上传请求的URL</param>
        /// <param name="file">上传的文件的信息</param>
        /// <param name="data">文件数据</param>
        /// <returns></returns>
        public static byte[] UploadFile(
            string url,
            FormFileParam file,
            Dictionary<string, string> data)
        {
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            var boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;

            List<byte> requestBytes = new List<byte>();


            if (data != null)
            {
                string formDataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (KeyValuePair<string, string> dict in data)
                {
                    requestBytes.AddRange(boundaryBytes);
                    string formItem = String.Format(formDataTemplate, dict.Key, dict.Value);
                    requestBytes.AddRange(Encoding.UTF8.GetBytes(formItem));
                }
            }
            requestBytes.AddRange(boundaryBytes);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            var header = String.Format(headerTemplate, file.ParamName, file.FileName, file.ContentType);
            var headerBytes = Encoding.UTF8.GetBytes(header);
            requestBytes.AddRange(headerBytes);

            requestBytes.AddRange(file.Data);

            byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            requestBytes.AddRange(trailer);

            var requestStream = request.GetRequestStream();
            requestStream.Write(requestBytes.ToArray(), 0, requestBytes.Count);
            requestStream.Close();

            var rep = request.GetResponse();
            using (Stream stream = rep.GetResponseStream())
            {
                if (stream == null) return null;
                List<byte> responseBytes = new List<byte>();
                byte[] buffer = new byte[1024];
                int read;
                do
                {
                    read = stream.Read(buffer, 0, buffer.Length);
                    responseBytes.AddRange(buffer.Take(read));
                } while (read > 0);

                return responseBytes.ToArray();
            }
        }


        /// <summary>
        /// 以Form表单的形式上传文件
        /// </summary>
        /// <param name="url">上传请求的URL</param>
        /// <param name="file">上传的文件的信息</param>
        /// <param name="data">文件数据</param>
        /// <returns></returns>
        public static async Task<byte[]> UploadFileAsync(
            string url,
            FormFileParam file,
            Dictionary<string, string> data)
        {
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            var boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;

            List<byte> requestBytes = new List<byte>();


            if (data != null)
            {
                string formDataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (KeyValuePair<string, string> dict in data)
                {
                    requestBytes.AddRange(boundaryBytes);
                    string formItem = String.Format(formDataTemplate, dict.Key, dict.Value);
                    requestBytes.AddRange(Encoding.UTF8.GetBytes(formItem));
                }
            }
            requestBytes.AddRange(boundaryBytes);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            var header = String.Format(headerTemplate, file.ParamName, file.FileName, file.ContentType);
            var headerBytes = Encoding.UTF8.GetBytes(header);
            requestBytes.AddRange(headerBytes);

            requestBytes.AddRange(file.Data);

            byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            requestBytes.AddRange(trailer);

            var requestStream = await request.GetRequestStreamAsync();
            //await requestStream.WriteAsync(requestBytes.ToArray(), 0, requestBytes.Count);
            requestStream.Write(requestBytes.ToArray(), 0, requestBytes.Count);
            requestStream.Close();

            var rep = await request.GetResponseAsync();
            using (Stream stream = rep.GetResponseStream())
            {
                if (stream == null) return null;
                List<byte> responseBytes = new List<byte>();
                byte[] buffer = new byte[1024];
                int read;
                do
                {
                    read = stream.Read(buffer, 0, buffer.Length);
                    responseBytes.AddRange(buffer.Take(read));
                } while (read > 0);

                return responseBytes.ToArray();
            }
        }


        #endregion
    }
}

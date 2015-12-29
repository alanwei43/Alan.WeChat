using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        public static void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
            }
            catch (Exception ex)
            {
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }
    }
}

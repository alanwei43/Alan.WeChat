using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Log;

namespace WeChat.Example.Api
{
    /// <summary>
    /// Summary description for DownloadImage
    /// </summary>
    public class DownloadImage : HttpTaskAsyncHandler
    {

        public async override Task ProcessRequestAsync(HttpContext context)
        {
            var req = context.Request;
            var rep = context.Response;
            var mediaId = req.QueryString["media"];
            var response = WeChat.Core.Api.ContensManage.DownloadMedia.Download(mediaId);
            if (response.ErrCode.GetValueOrDefault() != 0) return;

            rep.AddHeader("X-File-Ext", response.FileName);
            using (Stream write = rep.OutputStream)
                await write.WriteAsync(response.FileData, 0, response.FileData.Length);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
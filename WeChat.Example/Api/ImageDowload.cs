using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WeChat.Example.Api
{
    public class ImageDowload : HttpTaskAsyncHandler
    {

        public async override Task ProcessRequestAsync(HttpContext context)
        {
            var req = context.Request;
            var rep = context.Response;
            var mediaId = req.QueryString["media"];
            if (String.IsNullOrWhiteSpace(mediaId)) return;

            var response = WeChat.Core.Api.ContensManage.DownloadMedia.Download(mediaId);
            if (response.ErrCode.GetValueOrDefault() != 0) return;

            rep.AddHeader("X-File-Ext", response.FileName);
            using (Stream write = rep.OutputStream)
                await write.WriteAsync(response.FileData, 0, response.FileData.Length);
        }

    }
}
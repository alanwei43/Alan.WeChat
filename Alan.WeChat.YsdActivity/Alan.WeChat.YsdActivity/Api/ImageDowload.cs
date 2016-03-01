using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WeChat.YsdActivity.Api
{
    /// <summary>
    /// 根据media(即mediaId)从微信服务器上获取图片数据, 然后写入到响应流.
    /// </summary>
    public class ImageDowload : HttpTaskAsyncHandler
    {
        public async override Task ProcessRequestAsync(HttpContext context)
        {
            var req = context.Request;
            var rep = context.Response;
            var mediaId = req.QueryString["media"];
            if (String.IsNullOrWhiteSpace(mediaId)) return;

            //从微信服务器上获取图片数据
            var response = await WeChat.Core.Api.ContentsManage.DownloadTempMedia.DownloadAsync(mediaId);
            if (!response.IsSuccess) return;

            //返回给请求者
            rep.AddHeader("X-File-Ext", response.FileName);
            using (Stream write = rep.OutputStream)
                await write.WriteAsync(response.FileData, 0, response.FileData.Length);
        }

    }
}
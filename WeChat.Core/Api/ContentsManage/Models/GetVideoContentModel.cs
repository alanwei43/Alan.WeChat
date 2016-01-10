using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.Core.Api.Models;

namespace WeChat.Core.Api.ContentsManage.Models
{
    /// <summary>
    /// 获取永久素材 返回视频素材
    /// </summary>
    public class GetVideoContentModel : ResponseModel
    {
        public string title { get; set; }
        public string description { get; set; }
        public string down_url { get; set; }
    }
}

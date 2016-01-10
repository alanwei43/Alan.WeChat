using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.Core.Api.ContentsManage.Models;

namespace WeChat.Core.Api.ContentsManage.Models
{
    /// <summary>
    /// 媒体素材列表
    /// </summary>
    public class MediumContentsModel : ContentsBaseModel
    {
        public List<MediaContentModel> item { get; set; }
    }

    /// <summary>
    /// 媒体素材
    /// </summary>
    public class MediaContentModel
    {
        /// <summary>
        /// 素材Id
        /// </summary>
        public string media_id { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public long update_time { get; set; }

        /// <summary>
        /// 图文页的URL, 或者, 当获取的列表是图片素材列表时, 该字段是图片的URL
        /// </summary>
        public string url { get; set; }
    }
}

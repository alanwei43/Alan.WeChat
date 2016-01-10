using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.ContentsManage.Models
{
    /// <summary>
    /// 图文消息
    /// </summary>
    public class NewsItemModel
    {
        /// <summary>
        /// 图文消息的标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 图文消息的封面图片素材id(必须是永久mediaID)
        /// </summary>
        public string thumb_media_id { get; set; }

        /// <summary>
        /// 图文消息的封面图片的地址, 第三方开发者也可以使用这个URL下载图片到自己服务器中，然后显示在自己网站上
        /// </summary>
        public string thumb_url { get; set; }

        /// <summary>
        /// 是否显示封面, 0为false, 即不显示, 1为true, 即显示
        /// </summary>
        public string show_cover_pic { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string author { get; set; }

        /// <summary>
        /// 图文消息的摘要, 仅有单图文消息才有摘要, 多图文此处为空
        /// </summary>
        public string digest { get; set; }

        /// <summary>
        /// 图文消息的具体内容, 支持HTML标签, 必须少于2万字符, 小于1M, 且此处会去除JS
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 图文页的URL
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 图文消息的原文地址，即点击 "阅读原文" 后的URL
        /// </summary>
        public string content_source_url { get; set; }
    }
}

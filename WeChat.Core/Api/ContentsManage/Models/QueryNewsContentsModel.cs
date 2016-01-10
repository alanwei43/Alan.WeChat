using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.Core.Api.ContentsManage.Models;

namespace WeChat.Core.Api.ContentsManage.Models
{
    /// <summary>
    /// 获取素材列表 图文素材返回结果
    /// </summary>
    public class QueryNewsContentsModel : ContentsBaseModel
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public List<NewsListModel> item { get; set; }

        public class NewsListModel
        {
            /// <summary>
            /// 素材Id
            /// </summary>
            public string media_id { get; set; }

            /// <summary>
            /// 素材最后更新时间
            /// </summary>
            public long update_time { get; set; }

            /// <summary>
            /// 素材内容
            /// </summary>
            public NewsListContentModel content { get; set; }

            public class NewsListContentModel
            {
                /// <summary>
                /// 素材内容包含的文章列表
                /// </summary>
                public List<NewsItemModel> news_item { get; set; }
            }
        }

    }

    //public class NewsContentItemModel
    //{
    //    /// <summary>
    //    /// 图文消息的标题
    //    /// </summary>
    //    public string title { get; set; }

    //    /// <summary>
    //    /// 图文消息的封面图片素材id（必须是永久mediaID）
    //    /// </summary>
    //    public string thumb_media_id { get; set; }

    //    /// <summary>
    //    /// 是否显示封面，0为false，即不显示，1为true，即显示
    //    /// </summary>
    //    public string show_cover_pic { get; set; }

    //    /// <summary>
    //    /// 作者
    //    /// </summary>
    //    public string author { get; set; }

    //    /// <summary>
    //    /// 图文消息的摘要，仅有单图文消息才有摘要，多图文此处为空
    //    /// </summary>
    //    public string digest { get; set; }

    //    /// <summary>
    //    /// 图文消息的具体内容，支持HTML标签，必须少于2万字符，小于1M，且此处会去除JS
    //    /// </summary>
    //    public string content { get; set; }

    //    /// <summary>
    //    /// 图文页的URL，或者，当获取的列表是图片素材列表时，该字段是图片的URL
    //    /// </summary>
    //    public string url { get; set; }

    //    /// <summary>
    //    /// 图文消息的原文地址，即点击“阅读原文”后的URL
    //    /// </summary>
    //    public string content_source_url { get; set; }
    //}


}

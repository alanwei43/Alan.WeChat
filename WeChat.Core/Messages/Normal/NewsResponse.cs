using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WeChat.Core.Utils;

namespace WeChat.Core.Messages.Normal
{
    /// <summary>
    /// 回复图文消息
    /// </summary>
    public class NewsResponse : ResponseBase
    {
        public NewsResponse()
        {
            this.MsgType = Configurations.Current.MessageType.News;
        }
        /// <summary>
        /// 图文消息个数，限制为10条以内
        /// </summary>
        public int ArticleCount { get { return this.Articles.Count; } }

        /// <summary>
        /// 多条图文消息信息，默认第一个item为大图,注意，如果图文数超过10，则将会无响应
        /// </summary>
        public List<ArticleItem> Articles;

        /// <summary>
        /// 消息类型
        /// </summary>
        public new string MsgType { get; set; }

        /// <summary>
        /// 单个文章
        /// </summary>
        public class ArticleItem
        {
            /// <summary>
            /// 图文消息标题
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 图文消息描述
            /// </summary>
            public string Description { get; set; }
            /// <summary>
            /// 图片链接，支持JPG、PNG格式，较好的效果为大图360*200，小图200*200
            /// </summary>
            public string PicUrl { get; set; }
            /// <summary>
            /// 点击图文消息跳转链接
            /// </summary>
            public string Url { get; set; }
        }


        public override string ToXml()
        {
            if (Articles == null) this.Articles = new List<ArticleItem>();

            XElement ele = new XElement("xml",
                ModelBase.ToXmls(this),
                new XElement("Articles", this.Articles.Select(item => new XElement("item", ModelBase.ToXmls(item))))
                );

            return ele.ToString();
        }
    }
}

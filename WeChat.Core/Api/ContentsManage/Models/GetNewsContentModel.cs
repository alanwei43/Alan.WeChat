using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.Core.Api.Models;

namespace WeChat.Core.Api.ContentsManage.Models
{
    /// <summary>
    /// 获取永久素材 图文素材返回实体
    /// </summary>
    public class GetNewsContentModel : ResponseModel
    {
        /// <summary>
        /// 文章列表
        /// </summary>
        public List<NewsItemModel> news_item { get; set; }
    }
}

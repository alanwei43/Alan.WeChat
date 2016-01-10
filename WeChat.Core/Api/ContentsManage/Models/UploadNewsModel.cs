using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.ContentsManage.Models
{
    /// <summary>
    /// 新增永久图文素材 实体
    /// </summary>
    public class AddNewsModel
    {
        /// <summary>
        /// 文章列表
        /// </summary>
        public List<NewsItemModel> articles { get; set; }
    }
}

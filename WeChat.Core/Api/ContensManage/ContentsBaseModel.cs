using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.ContensManage
{
    public abstract class ContentsBaseModel
    {
        /// <summary>
        /// 该类型的素材的总数
        /// </summary>
        public long total_count { get; set; }

        /// <summary>
        /// 本次调用获取的素材的数量
        /// </summary>
        public int item_count { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public long? errcode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string errmsg { get; set; }
    }

}

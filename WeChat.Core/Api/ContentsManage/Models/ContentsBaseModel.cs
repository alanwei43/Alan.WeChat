using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.Core.Api.Models;

namespace WeChat.Core.Api.ContentsManage.Models
{
    public abstract class ContentsBaseModel:ResponseModel
    {
        /// <summary>
        /// 该类型的素材的总数
        /// </summary>
        public long total_count { get; set; }

        /// <summary>
        /// 本次调用获取的素材的数量
        /// </summary>
        public int item_count { get; set; }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.Models
{
    public class ResponseModel
    {

        /// <summary>
        /// 错误码
        /// </summary>
        public long? ErrCode { get; set; }
        /// <summary>
        /// 接口是否调用成功
        /// </summary>
        public bool IsSuccess { get { return this.ErrCode.GetValueOrDefault() == 0; } }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}

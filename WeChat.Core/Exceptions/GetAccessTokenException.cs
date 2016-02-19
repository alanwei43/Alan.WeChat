using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Exceptions
{
    /// <summary>
    /// 获取AccessToken异常
    /// </summary>
    public class GetAccessTokenException : Exception
    {

        /// <summary>
        /// 创建获取AccessToken异常信息
        /// </summary>
        /// <param name="apiDescription">接口调用说明</param>
        public GetAccessTokenException(string apiDescription) : base(String.Format("{0} 获取AccessToken时失败", apiDescription))
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Log
{
    public interface ILog
    {
        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="date">日期</param>
        /// <param name="category">分类</param>
        /// <param name="request">请求数据</param>
        /// <param name="response">输出数据</param>
        /// <param name="note">备注</param>
        void Write(string id, DateTime date, string category, string request, string response, string note);

    }

}

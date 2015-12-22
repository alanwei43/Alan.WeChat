using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Messages.Normal
{
    public class NormalBase : RequestBase
    {   /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long MsgId { get; set; }

    }
}

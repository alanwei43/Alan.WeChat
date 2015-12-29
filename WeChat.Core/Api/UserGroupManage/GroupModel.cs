using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.UserGroupManage
{
    /// <summary>
    /// 用户分组模型
    /// </summary>
    public class GroupModel
    {
        public GroupModel() { }
        public GroupModel(int groupId, string groupName)
        {
            this.id = groupId;
            this.name = groupName;
        }
        /// <summary>
        /// 分组id，由微信分配
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 分组名字，UTF8编码
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 分组内用户数量
        /// </summary>
        public int count { get; set; }
    }
}

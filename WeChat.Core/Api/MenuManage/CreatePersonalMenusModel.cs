using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.MenuManage
{
    public class CreatePersonalMenusModel
    {
        public List<CreateBaseMenuModel> button { get; set; }
    }

    /// <summary>
    /// 个性菜单 菜单匹配规则
    /// </summary>
    public class MatchRuleModel
    {
        /// <summary>
        /// 用户分组id，可通过用户分组管理接口获取
        /// </summary>
        public int group_id { get; set; }

        /// <summary>
        /// 性别: 男: 1, 女: 2, 不填则不做匹配
        /// </summary>
        public string sex { get; set; }
        public string country { get; set; }
        /// <summary>
        /// 省份信息，是用户在微信中设置的地区，具体请参考地区信息表
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 城市信息，是用户在微信中设置的地区，具体请参考地区信息表
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 客户端版本，当前只具体到系统型号：IOS(1), Android(2),Others(3) 不填则不做匹配
        /// </summary>
        public string client_platform_type { get; set; }
    }
}

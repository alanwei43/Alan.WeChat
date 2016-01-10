using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.MenuManage.Models
{
    /// <summary>
    /// 创建个性化菜单 实体
    /// </summary>
    public class CreatePersonalMenusModel
    {
        public List<CreateBaseMenuModel> button { get; set; }
    }

}

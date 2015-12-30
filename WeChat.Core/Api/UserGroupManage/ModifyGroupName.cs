using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alan.Utils.ExtensionMethods;

namespace WeChat.Core.Api.UserGroupManage
{
    /// <summary>
    /// 修改分组名
    /// </summary>
    public class ModifyGroupName : ApiBase
    {
        protected override async Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            return String.Format("https://api.weixin.qq.com/cgi-bin/groups/update?access_token={0}", token.Access_Token);
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            return String.Format("https://api.weixin.qq.com/cgi-bin/groups/update?access_token={0}", token.Access_Token);
        }
        protected override string ReqMethod
        {
            get { return "POST"; }
        }

        public ModifyGroupName() { }

        public ModifyGroupName(GroupModel group)
        {
            this.ReqData = Encoding.UTF8.GetBytes(new { group = group }.ExToJson());
        }

        public static ModifyGroupName Modify(GroupModel group)
        {
            var modify = new ModifyGroupName(group);
            var response = modify.RequestAsModel<ModifyGroupName>();
            return response;
        }
        public static async Task<ModifyGroupName> ModifyAsync(GroupModel group)
        {
            var modify = new ModifyGroupName(group);
            var response = await modify.RequestAsModelAsync<ModifyGroupName>();
            return response;
        }
    }
}

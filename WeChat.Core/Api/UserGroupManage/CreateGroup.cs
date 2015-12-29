using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alan.Utils.ExtensionMethods;

namespace WeChat.Core.Api.UserGroupManage
{
    /// <summary>
    /// 创建分组
    /// </summary>
    public class CreateGroup : ApiBase
    {
        protected override string ReqMethod
        {
            get { return "POST"; }
        }

        public CreateGroup()
        {
        }

        public CreateGroup(string groupName)
        {
            this.ReqData = new { group = new { name = groupName } }.ExToJson();
        }
        protected async override Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (!token.IsSuccess) throw new Exception(String.Format("创建分组时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/groups/create?access_token={0}", token.Access_Token);
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (!token.IsSuccess) throw new Exception(String.Format("创建分组时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/groups/create?access_token={0}", token.Access_Token);
        }
        public GroupModel group { get; set; }

        /// <summary>
        /// 创建分组
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <returns></returns>
        public static CreateGroup Create(string groupName)
        {
            var group = new CreateGroup(groupName);
            var response = group.RequestAsModel<CreateGroup>();
            return response;
        }
        /// <summary>
        /// 创建分组
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <returns></returns>
        public async static Task<CreateGroup> CreateAsync(string groupName)
        {
            var group = new CreateGroup(groupName);
            var response = await group.RequestAsModelAsync<CreateGroup>();
            return response;
        }
    }
}

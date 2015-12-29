using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alan.Utils.ExtensionMethods;

namespace WeChat.Core.Api.UserGroupManage
{
    /// <summary>
    /// 移动用户分组
    /// </summary>
    public class MoveUserToGroup : ApiBase
    {
        protected async override Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (!token.IsSuccess)
                throw new Exception(String.Format("移动用户分组 获取AccessToken时失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token={0}",
                token.Access_Token);
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (!token.IsSuccess)
                throw new Exception(String.Format("移动用户分组 获取AccessToken时失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token={0}",
                token.Access_Token);
        }
        protected override string ReqMethod
        {
            get { return "POST"; }
        }

        public MoveUserToGroup() { }

        /// <summary>
        /// 移动用户分组
        /// </summary>
        /// <param name="openId">用户唯一标识符</param>
        /// <param name="toGroupId">分组id</param>
        public MoveUserToGroup(string openId, int toGroupId)
        {
            this.ReqData = new { openid = openId, to_groupid = toGroupId }.ExToJson();
        }

        /// <summary>
        /// 移动用户分组
        /// </summary>
        /// <param name="openId">用户唯一标识符</param>
        /// <param name="toGroupId">分组id</param>
        /// <returns></returns>
        public static MoveUserToGroup Move(string openId, int toGroupId)
        {
            var move = new MoveUserToGroup(openId, toGroupId);
            var response = move.RequestAsModel<MoveUserToGroup>();
            return response;
        }

        /// <summary>
        /// 移动用户分组
        /// </summary>
        /// <param name="openId">用户唯一标识符</param>
        /// <param name="toGroupId">分组id</param>
        /// <returns></returns>
        public static async Task<MoveUserToGroup> MoveAsync(string openId, int toGroupId)
        {
            var move = new MoveUserToGroup(openId, toGroupId);
            var response = await move.RequestAsModelAsync<MoveUserToGroup>();
            return response;
        }
    }
}

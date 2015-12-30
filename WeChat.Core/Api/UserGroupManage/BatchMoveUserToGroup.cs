using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alan.Utils.ExtensionMethods;

namespace WeChat.Core.Api.UserGroupManage
{
    /// <summary>
    /// 批量移动用户分组
    /// </summary>
    public class BatchMoveUserToGroup : ApiBase
    {
        protected override string ReqMethod
        {
            get { return "POST"; }
        }
        protected override async Task<string> GetApiUrlAsync()
        {

            var token = await AccessToken.GetAsync();
            if (!token.IsSuccess) throw new Exception(String.Format("批量移动用户分组 获取AccessToken时失败: {0} {1}.", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/groups/members/batchupdate?access_token={0}",
                token.Access_Token);
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (!token.IsSuccess) throw new Exception(String.Format("批量移动用户分组 获取AccessToken时失败: {0} {1}.", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/groups/members/batchupdate?access_token={0}",
                token.Access_Token);
        }

        public BatchMoveUserToGroup() { }

        /// <summary>
        /// 批量移动用户分组
        /// </summary>
        /// <param name="openIdList">用户唯一标识符openid的列表（size不能超过50）</param>
        /// <param name="toGroupId">分组id</param>
        public BatchMoveUserToGroup(string[] openIdList, int toGroupId)
        {
            if (openIdList == null) throw new Exception("OpenIdList(用户唯一标识符openid的列表) 不能为空");
            if (openIdList.Length > 50) throw new Exception("OpenIdList(用户唯一标识符openid的列表) 数量不能超过50");
            this.ReqData = Encoding.UTF8.GetBytes(new { openid_list = openIdList, to_groupid = toGroupId }.ExToJson());
        }

        /// <summary>
        /// 批量移动用户分组
        /// </summary>
        /// <param name="openIdList">用户唯一标识符openid的列表（size不能超过50）</param>
        /// <param name="toGroupId">分组id</param>
        /// <returns></returns>
        public static BatchMoveUserToGroup Move(string[] openIdList, int toGroupId)
        {
            var move = new BatchMoveUserToGroup(openIdList, toGroupId);
            var response = move.RequestAsModel<BatchMoveUserToGroup>();
            return response;
        }
        /// <summary>
        /// 批量移动用户分组
        /// </summary>
        /// <param name="openIdList">用户唯一标识符openid的列表（size不能超过50）</param>
        /// <param name="toGroupId">分组id</param>
        /// <returns></returns>
        public static async Task<BatchMoveUserToGroup> MoveAsync(string[] openIdList, int toGroupId)
        {
            var move = new BatchMoveUserToGroup(openIdList, toGroupId);
            var response = await move.RequestAsModelAsync<BatchMoveUserToGroup>();
            return response;
        }
    }
}

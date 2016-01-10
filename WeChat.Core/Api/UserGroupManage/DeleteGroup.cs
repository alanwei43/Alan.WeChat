using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Api.Models;

namespace WeChat.Core.Api.UserGroupManage
{
    /// <summary>
    /// 删除分组
    /// </summary>
    public class DeleteGroup : ApiBase
    {
        protected override string ReqMethod
        {
            get { return "POST"; }
        }
        protected override async Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (!token.IsSuccess) throw new Exception(String.Format("删除分组 获取AccessToken时失败: {0} {1}", token.ErrCode, token.ErrMsg));

            return String.Format("https://api.weixin.qq.com/cgi-bin/groups/delete?access_token={0}", token.Access_Token);
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (!token.IsSuccess) throw new Exception(String.Format("删除分组 获取AccessToken时失败: {0} {1}", token.ErrCode, token.ErrMsg));

            return String.Format("https://api.weixin.qq.com/cgi-bin/groups/delete?access_token={0}", token.Access_Token);
        }

        public DeleteGroup() { }

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="groupId">分组Id</param>
        public DeleteGroup(int groupId)
        {
            this.ReqData = Encoding.UTF8.GetBytes(new { group = new { id = groupId } }.ExToJson());
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="groupId">分组Id</param>
        public static DeleteGroup Delete(int groupId)
        {
            var delete = new DeleteGroup(groupId);
            var response = delete.RequestAsModel<DeleteGroup>();
            return response;
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="groupId">分组Id</param>
        public static async Task<DeleteGroup> DeleteAsync(int groupId)
        {
            var delete = new DeleteGroup(groupId);
            var response = await delete.RequestAsModelAsync<DeleteGroup>();
            return response;
        }
    }
}

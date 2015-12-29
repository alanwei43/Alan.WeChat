using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alan.Utils.ExtensionMethods;

namespace WeChat.Core.Api.UserGroupManage
{
    /// <summary>
    /// 查询用户所在分组
    /// </summary>
    public class QueryGroupOfUser : ApiBase
    {
        protected async override Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (!token.IsSuccess) throw new Exception(String.Format("查询用户所在分组时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/groups/getid?access_token={0}", token.Access_Token);
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (!token.IsSuccess) throw new Exception(String.Format("查询用户所在分组时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/groups/getid?access_token={0}", token.Access_Token);
        }

        protected override string ReqMethod { get { return "POST"; } }
        public int groupid { get; set; }
        public QueryGroupOfUser() { }

        public QueryGroupOfUser(string openId)
        {
            this.ReqData = new { openid = openId }.ExToJson();
        }

        /// <summary>
        /// 查询用户所在的分组
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static QueryGroupOfUser Query(string openId)
        {
            var query = new QueryGroupOfUser(openId);
            var response = query.RequestAsModel<QueryGroupOfUser>();
            return response;
        }

        /// <summary>
        /// 查询用户所在的分组
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static async Task<QueryGroupOfUser> QueryAsync(string openId)
        {
            var query = new QueryGroupOfUser(openId);
            var response = await query.RequestAsModelAsync<QueryGroupOfUser>();
            return response;
        }
    }
}

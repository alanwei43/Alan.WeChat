using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.UserGroupManage
{
    /// <summary>
    /// 查询分组
    /// </summary>
    public class QueryGroups : ApiBase
    {
        protected async override Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (token.ErrCode != null && token.ErrCode != 0) throw new Exception(String.Format("查询分组时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/groups/get?access_token={0}", token.Access_Token);
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (token.ErrCode != null && token.ErrCode != 0) throw new Exception(String.Format("查询分组时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/groups/get?access_token={0}", token.Access_Token);
        }

        /// <summary>
        /// 公众平台分组信息列表
        /// </summary>
        public List<GroupModel> groups { get; set; }

        /// <summary>
        /// 查询分组
        /// </summary>
        /// <returns></returns>
        public static QueryGroups Query()
        {
            var query = new QueryGroups();
            var response = query.RequestAsModel<QueryGroups>();
            return response;
        }

        /// <summary>
        /// 查询分组
        /// </summary>
        /// <returns></returns>
        public static async Task<QueryGroups> QueryAsync()
        {
            var query = new QueryGroups();
            var response = await query.RequestAsModelAsync<QueryGroups>();
            return response;
        }
    }
}

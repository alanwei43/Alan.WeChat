using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.UserGroupManage
{
    /// <summary>
    /// 获取用户列表
    /// </summary>
    public class QueryUsers : ApiBase
    {
        private string _nextOpenId;
        protected override string ReqMethod
        {
            get { return "POST"; }
        }
        protected override async Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (token.IsSuccess)
                throw new Exception(String.Format("获取用户列表 获取AccessToken时失败:{0} {1}.", token.ErrCode, token.ErrMsg));

            if (String.IsNullOrWhiteSpace(this._nextOpenId))
                return String.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}", token.Access_Token);

            return
                String.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}", token.Access_Token, this._nextOpenId);
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (token.IsSuccess)
                throw new Exception(String.Format("获取用户列表 获取AccessToken时失败:{0} {1}.", token.ErrCode, token.ErrMsg));

            if (String.IsNullOrWhiteSpace(this._nextOpenId))
                return String.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}", token.Access_Token);

            return
                String.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}", token.Access_Token, this._nextOpenId);
        }
        public QueryUsers() { }

        public QueryUsers(string nextOpenId)
        {
            this._nextOpenId = nextOpenId;
        }

        public static QueryUsers Query()
        {
            var q = new QueryUsers(null);
            var response = q.RequestAsModel<QueryUsers>();
            return response;
        }

        public static async Task<QueryUsers> QueryAsync()
        {
            var q = new QueryUsers(null);
            var response = await q.RequestAsModelAsync<QueryUsers>();
            return response;
        }

        public static QueryUsers Query(string nextOpenId)
        {
            var q = new QueryUsers(nextOpenId);
            var response = q.RequestAsModel<QueryUsers>();
            return response;
        }

        public static async Task<QueryUsers> QueryAsync(string nextOpenId)
        {
            var q = new QueryUsers(nextOpenId);
            var response = await q.RequestAsModelAsync<QueryUsers>();
            return response;
        }
    }
}

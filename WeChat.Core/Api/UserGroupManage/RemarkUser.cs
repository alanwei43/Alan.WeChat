using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alan.Utils.ExtensionMethods;

namespace WeChat.Core.Api.UserGroupManage
{
    /// <summary>
    /// 设置备注名
    /// </summary>
    public class RemarkUser : ApiBase
    {
        protected override string ReqMethod
        {
            get { return "POST"; }
        }
        protected override async Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (!token.IsSuccess) throw new Exception(String.Format("设置备注名 获取AccessToken时失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/user/info/updateremark?access_token={0}", token.Access_Token);
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (!token.IsSuccess) throw new Exception(String.Format("设置备注名 获取AccessToken时失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/user/info/updateremark?access_token={0}", token.Access_Token);
        }
        public RemarkUser() { }

        /// <summary>
        /// 设置备注名
        /// </summary>
        /// <param name="openId">用户标识</param>
        /// <param name="remark">新的备注名，长度必须小于30字符</param>
        public RemarkUser(string openId, string remark)
        {
            this.ReqData = Encoding.UTF8.GetBytes(new { openid = openId, remark = remark }.ExToJson());
        }


        /// <summary>
        /// 设置备注名
        /// </summary>
        /// <param name="openId">用户标识</param>
        /// <param name="remark">新的备注名，长度必须小于30字符</param>
        /// <returns></returns>
        public static RemarkUser Remark(string openId, string remark)
        {
            var remarkUser = new RemarkUser(openId, remark);
            var response = remarkUser.RequestAsModel<RemarkUser>();
            return response;
        }

        /// <summary>
        /// 设置备注名
        /// </summary>
        /// <param name="openId">用户标识</param>
        /// <param name="remark">新的备注名，长度必须小于30字符</param>
        /// <returns></returns>
        public static async Task<RemarkUser> RemarkAsync(string openId, string remark)
        {
            var remarkUser = new RemarkUser(openId, remark);
            var response = await remarkUser.RequestAsModelAsync<RemarkUser>();
            return response;
        }
    }
}

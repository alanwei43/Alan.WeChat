using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Api.Models;
using WeChat.Core.Api.Nearby.Models;
using WeChat.Core.Exceptions;

namespace WeChat.Core.Api.Nearby
{
    /// <summary>
    /// 申请开通微信周边摇一摇功能
    /// </summary>
    public class ApplyFor : ApiBase
    {
        protected async override Task<string> GetApiUrlAsync()
        {

            var token = await AccessToken.GetAsync();
            if (!token.IsSuccess) throw new GetAccessTokenException("申请开通周边摇一摇");
            return String.Format("https://api.weixin.qq.com/shakearound/account/register?access_token={0}", token.Access_Token);
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (!token.IsSuccess) throw new GetAccessTokenException("申请开通周边摇一摇");
            return String.Format("https://api.weixin.qq.com/shakearound/account/register?access_token={0}", token.Access_Token);
        }


        protected override string ReqMethod { get { return "POST"; } }

        protected override byte[] ReqData { get; set; }

        internal ApplyFor() { }

        /// <summary>
        ///  创建申请
        /// </summary>
        /// <param name="model">申请信息</param>
        public ApplyFor(ApplyForModel model)
        {
            this.ReqData = Encoding.UTF8.GetBytes(model.ExToJson());
        }

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <returns></returns>
        public ApplyFor Submit()
        {
            return this.RequestAsModel<ApplyFor>();
        }

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <returns></returns>
        public async Task<ApplyFor> SubmitAsync()
        {
            return await this.RequestAsModelAsync<ApplyFor>();
        }

    }
}

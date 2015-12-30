using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.ContensManage
{
    /// <summary>
    /// 新增永久素材
    /// </summary>
    public class UploadLongTermMedia : ApiBase
    {
        protected override async Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (!token.IsSuccess) throw new Exception(String.Format("新增永久素材时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/material/add_news?access_token={0}",
                token.Access_Token);
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (!token.IsSuccess) throw new Exception(String.Format("新增永久素材时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/material/add_news?access_token={0}",
                token.Access_Token);
        }
    }
}

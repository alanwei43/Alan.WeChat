using Alan.Utils.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.Core.Api.Models;

namespace WeChat.Core.Api.ContentsManage
{
    /// <summary>
    /// 删除永久素材
    /// </summary>
    public class DeleteContent : ApiBase
    {
        protected override string ReqMethod
        {
            get
            {
                return "POST";
            }
        }
        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (!token.IsSuccess) throw new Exception(String.Format("删除永久素材时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/material/del_material?access_token={0}", token.Access_Token);
        }

        protected override async Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (!token.IsSuccess) throw new Exception(String.Format("删除永久素材时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/material/del_material?access_token={0}", token.Access_Token);
        }

        public DeleteContent() { }
        /// <summary>
        /// 删除素材 
        /// </summary>
        /// <param name="mediaId">素材Id</param>
        public DeleteContent(string mediaId)
        {
            this.ReqData = Encoding.UTF8.GetBytes(new { media_id = mediaId }.ExToJson());
        }

        /// <summary>
        /// 删除素材
        /// </summary>
        /// <param name="mediaId">素材Id</param>
        /// <returns></returns>
        public static DeleteContent Delete(string mediaId)
        {
            var delete = new DeleteContent(mediaId);
            var response = delete.RequestAsModel<DeleteContent>();
            return response;
        }
        /// <summary>
        /// 删除素材
        /// </summary>
        /// <param name="mediaId">素材Id</param>
        /// <returns></returns>
        public static async Task<DeleteContent> DeleteAsync(string mediaId)
        {
            var delete = new DeleteContent(mediaId);
            var response = await delete.RequestAsModelAsync<DeleteContent>();
            return response;
        }
    }
}

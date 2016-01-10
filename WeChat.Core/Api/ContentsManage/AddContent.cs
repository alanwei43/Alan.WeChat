using Alan.Utils.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.Core.Api.ContentsManage.Models;
using WeChat.Core.Api.Models;

namespace WeChat.Core.Api.ContentsManage
{
    /// <summary>
    /// 新增永久图文素材
    /// </summary>
    public class AddNews : ApiBase
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
            if (!token.IsSuccess) throw new Exception(String.Format("新增永久图文素材时获取AccessToken失败: {0} {1}.", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/material/add_news?access_token={0}", token.Access_Token);
        }

        protected override async Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (!token.IsSuccess) throw new Exception(String.Format("新增永久图文素材时获取AccessToken失败: {0} {1}.", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/material/add_news?access_token={0}", token.Access_Token);
        }

        public AddNews() { }
        /// <summary>
        /// 新增永久图文素材
        /// </summary>
        /// <param name="model">图文素材</param>
        public AddNews(AddNewsModel model)
        {
            if (model == null || model.articles == null) throw new Exception("数据无效");
            if (!model.articles.Any()) throw new Exception("文章列表不能为空");
            this.ReqData = Encoding.UTF8.GetBytes(model.ExToJson());
        }


        /// <summary>
        /// 新增永久图文素材
        /// </summary>
        /// <param name="model">图文素材</param>
        /// <return></return>
        public static AddNews Add(AddNewsModel model)
        {
            var add = new AddNews(model);
            var response = add.RequestAsModel<AddNews>();
            return response;
        }

        /// <summary>
        /// 新增永久图文素材
        /// </summary>
        /// <param name="model">图文素材</param>
        /// <returns></returns>
        public static Task<AddNews> AddAsync(AddNewsModel model)
        {
            var add = new AddNews(model);
            var response = add.RequestAsModelAsync<AddNews>();
            return response;
        }


    }
}

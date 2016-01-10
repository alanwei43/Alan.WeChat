using Alan.Utils.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.Core.Api.MenuManage.Models;
using WeChat.Core.Api.Models;

namespace WeChat.Core.Api.MenuManage
{
    /// <summary>
    /// 创建个性化菜单
    /// </summary>
    public class CreatePersonalMenus : ApiBase
    {
        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (!token.IsSuccess) throw new Exception(String.Format("创建个性化菜单时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/menu/addconditional?access_token={0}", token.Access_Token);
        }

        protected override async Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (!token.IsSuccess) throw new Exception(String.Format("创建个性化菜单时获取AccessToken失败: {0} {1}", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/menu/addconditional?access_token={0}", token.Access_Token);
        }
        protected override string ReqMethod
        {
            get
            {
                return "POST";
            }
        }
        public CreatePersonalMenus() { }
        public CreatePersonalMenus(CreatePersonalMenusModel model)
        {
            this.ReqData = Encoding.UTF8.GetBytes(model.ExToJson());
        }
        public CreatePersonalMenus(Dictionary<string, object> model)
        {
            this.ReqData = Encoding.UTF8.GetBytes(model.ExToJson());
        }
        public CreatePersonalMenus(string model)
        {
            this.ReqData = Encoding.UTF8.GetBytes(model);
        }

        public static CreatePersonalMenus Create(CreatePersonalMenusModel model)
        {
            var create = new CreatePersonalMenus(model);
            return create.RequestAsModel<CreatePersonalMenus>();
        }
        public static async Task<CreatePersonalMenus> CreateAsync(CreatePersonalMenusModel model)
        {
            var create = new CreatePersonalMenus(model);
            return await create.RequestAsModelAsync<CreatePersonalMenus>();
        }
        public static CreatePersonalMenus Create(string model)
        {
            var create = new CreatePersonalMenus(model);
            return create.RequestAsModel<CreatePersonalMenus>();
        }
        public static async Task<CreatePersonalMenus> CreateAsync(string model)
        {
            var create = new CreatePersonalMenus(model);
            return await create.RequestAsModelAsync<CreatePersonalMenus>();
        }
        public static CreatePersonalMenus Create(Dictionary<string, object> model)
        {
            var create = new CreatePersonalMenus(model);
            return create.RequestAsModel<CreatePersonalMenus>();
        }
        public static async Task<CreatePersonalMenus> CreateAsync(Dictionary<string, object> model)
        {
            var create = new CreatePersonalMenus(model);
            return await create.RequestAsModelAsync<CreatePersonalMenus>();
        }
    }
}

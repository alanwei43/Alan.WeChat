using Alan.Utils.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.MenuManage
{
    /// <summary>
    /// 删除个性化菜单 
    /// </summary>
    public class DeletePersonalMenus : ApiBase
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
            if (!token.IsSuccess) throw new Exception(String.Format("删除个性化菜单时获取AccessToken失败: {0} {1}.", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/menu/delconditional?access_token={0}", token.Access_Token);
        }

        protected override async Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (!token.IsSuccess) throw new Exception(String.Format("删除个性化菜单时获取AccessToken失败: {0} {1}.", token.ErrCode, token.ErrMsg));
            return String.Format("https://api.weixin.qq.com/cgi-bin/menu/delconditional?access_token={0}", token.Access_Token);
        }

        public DeletePersonalMenus() { }

        /// <summary>
        /// menuid为菜单id，可以通过自定义菜单查询接口获取
        /// </summary>
        /// <param name="menuId"></param>
        public DeletePersonalMenus(int menuId)
        {
            this.ReqData = new { menuid = menuId }.ExToJson();
        }
        public static DeletePersonalMenus Delete(int menuId)
        {
            var delete = new DeletePersonalMenus(menuId);
            var response = delete.RequestAsModel<DeletePersonalMenus>();
            return response;
        }
        public static async Task<DeletePersonalMenus> DeleteAsync(int menuId)
        {
            var delete = new DeletePersonalMenus(menuId);
            var response = await delete.RequestAsModelAsync<DeletePersonalMenus>();
            return response;
        }
    }
}

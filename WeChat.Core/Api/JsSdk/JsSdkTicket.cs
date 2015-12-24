using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WeChat.Core.Utils;

namespace WeChat.Core.Api.JsSdk
{

    public class JsSdkTicket : ApiBase
    {
        /// <summary>
        /// jsapi, wxcard
        /// </summary>
        private string _jsSdkType;
        public string Ticket { get; set; }
        public long Expires_In { get; set; }
        protected async override Task<string> GetApiUrlAsync()
        {
            var token = await AccessToken.GetAsync();
            if (token.ErrCode != null && token.ErrCode.Value != 0) throw new Exception(String.Format("获取AccessToken失败: {0} {1}.", token.ErrCode, token.ErrMsg));
            return
                String.Format(
                    "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type={1}",
                    token.Access_Token, this._jsSdkType);
        }

        protected override string GetApiUrl()
        {
            var token = AccessToken.Get();
            if (token.ErrCode != null && token.ErrCode.Value != 0) throw new Exception(String.Format("获取AccessToken失败: {0} {1}.", token.ErrCode, token.ErrMsg));
            return
                String.Format(
                    "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type={1}",
                    token.Access_Token, this._jsSdkType);
        }

        public JsSdkTicket() { }
        public JsSdkTicket(string jsSdkType)
        {
            this._jsSdkType = jsSdkType;
        }

        public static JsSdkConfigModel Get(string jsSdkType, string url)
        {
            var sdk = new JsSdkTicket(jsSdkType);
            var response = sdk.RequestAsModel<JsSdkTicket>();
            if (response.ErrCode != null && response.ErrCode.Value != 0) throw new Exception(String.Format("获取JsSdk Ticket失败: {0} {1}", sdk.ErrCode, sdk.ErrMsg));

            SortedDictionary<string, string> keyValues =
                new SortedDictionary<string, string>();

            keyValues.Add("noncestr", Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16));
            keyValues.Add("jsapi_ticket", response.Ticket);
            keyValues.Add("timestamp", Configurations.Current.GetTimeStamp().ToString());
            keyValues.Add("url", url);

            StringBuilder sb = new StringBuilder();
            foreach (var kv in keyValues)
            {
                sb.AppendFormat("{0}={1}&", kv.Key, kv.Value);
            }
            sb.Remove(sb.Length - 1, 1);


            SHA1 sha = new SHA1CryptoServiceProvider();
            var hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));



            var configModel = new JsSdkConfigModel()
            {
                AppId = Configurations.Current.AppId,
                RandomString = keyValues["noncestr"],
                Signature = BitConverter.ToString(hashValue).Replace("-", "").ToLower(),
                TimeStamp = keyValues["timestamp"]
            };
            configModel.Other = new { Para = sb.ToString(), Hash = configModel.Signature };

            return configModel;
        }
    }
}

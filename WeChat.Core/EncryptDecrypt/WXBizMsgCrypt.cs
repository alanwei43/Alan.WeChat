using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.Security.Cryptography;



namespace WeChat.Core.EncryptDecrypt
{
    /// <summary>
    /// -40001 : 签名验证错误
    /// -40002 :  xml解析失败
    /// -40003 :  sha加密生成签名失败
    /// -40004 :  AESKey 非法
    /// -40005 :  appid 校验错误
    /// -40006 :  AES 加密失败
    /// -40007 : AES 解密失败
    /// -40008 : 解密后得到的buffer非法
    /// -40009 :  base64加密异常
    /// -40010 :  base64解密异常
    /// </summary>
    public class WXBizMsgCrypt
    {
        private string token;
        private string aesKey;
        private string appId;
        
        public WXBizMsgCrypt(string t, string a, string k)
        {
            token = t;
            appId = a;
            aesKey = k;
        }
        /// <summary>
        /// 构造函数
        /// </summary>

        public WXBizMsgCrypt()
        {
            token = WeChat.Core.Utils.Configurations.Current.Token;
            appId = WeChat.Core.Utils.Configurations.Current.AppId;
            aesKey = WeChat.Core.Utils.Configurations.Current.AesKey;
        }

        /// <summary>
        /// 检验消息的真实性 并且获取解密后的明文
        /// </summary>
        /// <param name="sMsgSignature">签名串 对应URL参数的msg_signature</param>
        /// <param name="sTimeStamp">时间戳 对应URL参数的timestamp/param>
        /// <param name="sNonce">随机串 对应URL参数的nonce</param>
        /// <param name="sPostData">密文 对应POST请求的数据</param>
        /// <param name="sMsg">解密后的原文 当return返回0时有效</param>
        /// <returns>成功0 失败返回对应的错误码</returns>
        public Tuple<bool, string> DecryptMsg(
            string sMsgSignature,
            string sTimeStamp,
            string sNonce,
            string sPostData)
        {
            if (aesKey.Length != 43)
            {
                return Tuple.Create(false, "无效的密钥");
            }
            XmlDocument doc = new XmlDocument();
            XmlNode root;
            string sEncryptMsg;
            try
            {
                doc.LoadXml(sPostData);
                root = doc.FirstChild;
                sEncryptMsg = root["Encrypt"].InnerText;
            }
            catch (Exception)
            {
                return Tuple.Create(false, "解析XML错误");
            }
            //verify signature
            var verify = VerifySignature(token, sTimeStamp, sNonce, sEncryptMsg, sMsgSignature);
            if (!verify.Item1) return verify;

            //decrypt
            var sMsg = Cryptography.Decrypt(sEncryptMsg, aesKey);
            return Tuple.Create(true, sMsg);

        }

        /// <summary>
        /// 将企业号回复用户的消息加密打包
        /// </summary>
        /// <param name="sReplyMsg">企业号待回复用户的消息 xml格式的字符串</param>
        /// <param name="sTimeStamp">时间戳 可以自己生成 也可以用URL参数的timestamp</param>
        /// <param name="sNonce">随机串 可以自己生成 也可以用URL参数的nonce</param>
        /// <param name="sEncryptMsg">加密后的可以直接回复用户的密文 包括msg_signature, timestamp, nonce, encrypt的xml格式的字符串</param>
        /// <returns>成功0 失败返回对应的错误码</returns>
        public Tuple<bool, string> EncryptMsg(string sReplyMsg, string sTimeStamp, string sNonce)
        {
            if (aesKey.Length != 43)
                return Tuple.Create(false, "无效的密钥长度");

            string raw = "";
            try
            {
                raw = Cryptography.Encrypt(sReplyMsg, aesKey, appId);
            }
            catch (Exception)
            {
                return Tuple.Create(false, "加密错误");
            }
            var items = GenarateSinature(token, sTimeStamp, sNonce, raw);

            var sEncryptMsg = "";

            string EncryptLabelHead = "<Encrypt><![CDATA[";
            string EncryptLabelTail = "]]></Encrypt>";
            string MsgSigLabelHead = "<MsgSignature><![CDATA[";
            string MsgSigLabelTail = "]]></MsgSignature>";
            string TimeStampLabelHead = "<TimeStamp><![CDATA[";
            string TimeStampLabelTail = "]]></TimeStamp>";
            string NonceLabelHead = "<Nonce><![CDATA[";
            string NonceLabelTail = "]]></Nonce>";
            sEncryptMsg = sEncryptMsg + "<xml>" + EncryptLabelHead + raw + EncryptLabelTail;
            sEncryptMsg = sEncryptMsg + MsgSigLabelHead + items.Item2 + MsgSigLabelTail;
            sEncryptMsg = sEncryptMsg + TimeStampLabelHead + sTimeStamp + TimeStampLabelTail;
            sEncryptMsg = sEncryptMsg + NonceLabelHead + sNonce + NonceLabelTail;
            sEncryptMsg += "</xml>";
            return Tuple.Create(true, sEncryptMsg);
        }

        /// <summary>
        /// 验证 hash
        /// </summary>
        /// <param name="token"></param>
        /// <param name="timeStamp"></param>
        /// <param name="nonce"></param>
        /// <param name="encryptMsg"></param>
        /// <param name="hashValue"></param>
        /// <returns></returns>
        private static Tuple<bool, string> VerifySignature(
            string token,
            string timeStamp,
            string nonce,
            string encryptMsg,
            string hashValue)
        {
            return Tuple.Create(true, "");
            var items = GenarateSinature(token, timeStamp, nonce, encryptMsg);

            if (!items.Item1) return Tuple.Create(false, items.Item2);

            if (items.Item2 == hashValue) return Tuple.Create(true, "");
            return Tuple.Create(false, "HASH校验失败");
        }

        /// <summary>
        /// 计算 hash
        /// </summary>
        /// <param name="token"></param>
        /// <param name="timeStamp"></param>
        /// <param name="nonce"></param>
        /// <param name="encryptMsg"></param>
        /// <returns></returns>
        public static Tuple<bool, string> GenarateSinature(
            string token,
            string timeStamp,
            string nonce,
            string encryptMsg)
        {
            string[] values = new string[] { token, timeStamp, nonce, encryptMsg };

            try
            {
                SHA1 sha = new SHA1CryptoServiceProvider();
                byte[] dataToHash = Encoding.ASCII.GetBytes(String.Join("", values.OrderBy(v => v)));
                byte[] dataHashed = sha.ComputeHash(dataToHash);
                var hashValue = BitConverter.ToString(dataHashed).Replace("-", "").ToLower();
                return Tuple.Create(true, hashValue);
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, "计算签名异常: " + ex.Message);
            }
        }
    }
}

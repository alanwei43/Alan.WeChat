using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Api.Nearby.Models
{
    /// <summary>
    /// 申请开通周边摇一摇功能模型
    /// </summary>
    public class ApplyForModel
    {
        /// <summary>
        /// 调用接口凭证
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 联系人姓名，不超过20汉字或40个英文字母
        /// </summary>
        public string name { get; set; }


        /// <summary>
        /// 联系人电话
        /// </summary>
        public string phone_number { get; set; }

        /// <summary>
        /// 联系人邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 平台定义的行业代号，具体请查看行业代号
        /// </summary>
        public string industry_id { get; set; }

        /// <summary>
        /// 相关资质文件的图片url  图片需先上传至微信侧服务器, 用"素材管理-上传图片素材" 接口上传图片 返回的图片URL再配置在此处
        /// 当不需要资质文件时，数组内可以不填写url
        /// </summary>
        public string qualification_cert_urls { get; set; }

        /// <summary>
        /// 申请理由, 不超过250汉字或500个英文字母
        /// </summary>
        public string apply_reason { get; set; }


    }
}

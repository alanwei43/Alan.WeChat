﻿namespace WeChat.Core.Messages.Middlewares
{
    public class MiddlewareOutput
    {
        /// <summary>
        /// 输出强类型
        /// </summary>
        public ResponseBase ResponseModel { get; set; }

        /// <summary>
        /// 输出
        /// </summary>
        public string Response
        {
            get
            {
                if (ResponseModel == null) return "";
                return ResponseModel.ToXml();
            }
        }

    }
}

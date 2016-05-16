using Alan.Utils.ExtensionMethods;

namespace WeChat.Core.Messages.Middlewares
{
    public class MiddlewareInput
    {
        /// <summary>
        /// 原始请求输入
        /// </summary>
        public string RawRequest { get; private set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        public RequestBase RequestBaseModel { get; private set; }

        public MiddlewareInput(string req, RequestBase model)
        {
            this.RawRequest = req;
            this.RequestBaseModel = model;
        }

        /// <summary>
        /// 获取请求类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetRequestModel<T>()
            where T : RequestBase
        {
            var model = this.RawRequest.ExXmlToEntity<T>();
            if (model != null)
            {
                model.Signature = this.RequestBaseModel.Signature;
                model.Nonce = this.RequestBaseModel.Nonce;
                model.Timestamp = this.RequestBaseModel.Timestamp;
            }
            return model;
        }

    }
}

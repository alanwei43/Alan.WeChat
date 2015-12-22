using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Log;

namespace WeChat.Core.Messages
{
    /// <summary>
    /// 中间件
    /// </summary>
    public class Middleware

    {

        //public static T Resolve<T>(System.Web.HttpRequest request)
        //    where T : RequestBase
        //{
        //    using (StreamReader reader = new StreamReader(request.InputStream))
        //    {
        //        var input = reader.ReadToEnd();
        //        var model = Resolve<T>(input);
        //        model.CreateTime = request.Params["createtime"];
        //        model.Nonce = request.Params["nonce"];
        //        model.Signature = request.Params["signature"];
        //        return model;
        //    }
        //}

        //public static T Resolve<T>(string input)
        //    where T : RequestBase
        //{
        //    var model = input.ExXmlToEntity<T>();
        //    return model;
        //}

        /// <summary>
        /// 中间件过滤器
        /// </summary>
        private static List<Func<MiddlewareParameter, MiddlewareParameter>> Filters { get; set; }

        /// <summary>
        /// 注入过滤器
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns>过滤器索引</returns>
        public static int Inject(Func<MiddlewareParameter, MiddlewareParameter> filter)
        {
            if (Filters == null) Filters = new List<Func<MiddlewareParameter, MiddlewareParameter>>();

            Filters.Add(filter);
            return Filters.Count;
        }

        /// <summary>
        /// 移除过滤器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool Remove(int index)
        {
            if (Filters == null) return false;
            if (Filters.Count < index) return false;
            Filters.RemoveAt(index);
            return true;
        }

        public static MiddlewareParameter Execute(string input, string signature, string nonce, string timestamp)
        {
            LogUtils.Current.WriteWithOutId(category: "/Message/Request", note: input);

            var requestModel = input.ExXmlToEntity<RequestBase>();
            requestModel.Nonce = nonce;
            requestModel.Timestamp = timestamp;
            requestModel.Signature = signature;

            var middleInput = new MiddlewareParameter
            {
                Input = new MiddlewareInput()
                {
                    Request = input,
                    RequestBaseModel = requestModel
                },
                Output = new MiddlewareOutput()
                {
                    Response = "",
                    ResponseModel = new ResponseBase()
                }
            };

            return Execute(middleInput);
        }

        public static MiddlewareParameter Execute(System.Web.HttpRequest request)
        {
            using (StreamReader reader = new StreamReader(request.InputStream))
            {
                var requestInput = reader.ReadToEnd();
                return Execute(requestInput, request.QueryString["signature"], request.QueryString["nonce"], request.QueryString["timestamp"]);
            }
        }

        /// <summary>
        /// 执行过滤器
        /// </summary>
        /// <returns></returns>
        private static MiddlewareParameter Execute(MiddlewareParameter middleParameter)
        {
            return Filters.Aggregate(middleParameter, (current, filter) => filter(current));
        }
    }
}

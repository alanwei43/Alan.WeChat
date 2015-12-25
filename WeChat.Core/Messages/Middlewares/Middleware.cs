using System;
using System.Collections.Generic;
using System.IO;
using Alan.Utils.ExtensionMethods;
using WeChat.Core.Log;
using WeChat.Core.Messages.Events;
using WeChat.Core.Messages.Normal;
using WeChat.Core.Utils;
using WeChat.Core.Messages;

namespace WeChat.Core.Messages.Middlewares
{

    /// <summary>
    /// 中间件
    /// </summary>
    public sealed class Middleware
    {
        /// <summary>
        /// 全局开始过滤器
        /// </summary>
        private static readonly List<Action<MiddlewareParameter>> GlobalPreFilters;

        /// <summary>
        /// 全局结束过滤器
        /// </summary>
        private static readonly List<Action<MiddlewareParameter>> GlobalEndFilters;

        /// <summary>
        /// 文本消息过滤器
        /// </summary>
        public static FiltersContainer<TextRequest> TextFilters { get; private set; }

        /// <summary>
        /// 图片消息过滤器
        /// </summary>
        public static FiltersContainer<ImageRequest> ImageFilters { get; private set; }

        /// <summary>
        /// 位置消息过滤去
        /// </summary>
        public static FiltersContainer<LocationRequest> LocationFilters { get; private set; }

        /// <summary>
        /// 扫描二维码事件过滤器
        /// </summary>
        public static FiltersContainer<ScanQrRequest> ScanQrFilters { get; private set; }

        /// <summary>
        /// 位置事件过滤器
        /// </summary>
        public static FiltersContainer<PositionRequest> PositionFilter { get; private set; }

        /// <summary>
        /// 单击菜单事件过滤器
        /// </summary>
        public static FiltersContainer<ClickMenuRequest> ClickFilters { get; private set; }

        /// <summary>
        /// 事件过滤器
        /// </summary>
        public static FiltersContainer<EventBase> EventFileters { get; private set; }

        static Middleware()
        {
            GlobalPreFilters = new List<Action<MiddlewareParameter>>();
            GlobalEndFilters = new List<Action<MiddlewareParameter>>();

            TextFilters = new FiltersContainer<TextRequest>();
            ImageFilters = new FiltersContainer<ImageRequest>();
            LocationFilters = new FiltersContainer<LocationRequest>();
            ScanQrFilters = new FiltersContainer<ScanQrRequest>();
            PositionFilter = new FiltersContainer<PositionRequest>();
            ClickFilters = new FiltersContainer<ClickMenuRequest>();
            EventFileters = new FiltersContainer<EventBase>();
        }

        #region 注入过滤器

        /// <summary>
        /// 注入过滤器
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns>过滤器索引</returns>
        public static int InjectGlobalPreFilter(Action<MiddlewareParameter> filter)
        {
            GlobalPreFilters.Add(filter);
            return GlobalPreFilters.Count - 1;
        }
        /// <summary>
        /// 注入过滤器
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns>过滤器索引</returns>
        public static int InjectGlobalEndFilter(Action<MiddlewareParameter> filter)
        {
            GlobalEndFilters.Add(filter);
            return GlobalEndFilters.Count - 1;
        }

        #endregion

        /// <summary>
        /// 移除过滤器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool RemoveGlobalFilter(int index)
        {
            if (GlobalPreFilters == null) return false;
            if (GlobalPreFilters.Count < index) return false;
            GlobalPreFilters.RemoveAt(index);
            return true;
        }

        #region 执行过滤器

        /// <summary>
        /// 执行过滤器
        /// </summary>
        /// <param name="input"></param>
        /// <param name="signature"></param>
        /// <param name="nonce"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static MiddlewareParameter Execute(string input, string signature, string nonce, string timestamp)
        {
            var requestModel = input.ExXmlToEntity<RequestBase>();
            requestModel.Nonce = nonce;
            requestModel.Timestamp = timestamp;
            requestModel.Signature = signature;

            var middleInput = new MiddlewareParameter
            {
                Input = new MiddlewareInput(input, requestModel)
            };

            return Execute(middleInput);
        }

        /// <summary>
        /// 执行过滤器
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
        private static MiddlewareParameter Execute(MiddlewareParameter middlareResult)
        {
            //全局开始过滤器
            GlobalPreFilters.ForEach(filter => filter(middlareResult));

            //处理文本消息
            if (middlareResult.Input.RequestBaseModel.MsgType == Configurations.Current.MessageType.Text)
            {
                var textReq = middlareResult.Input.GetRequestModel<TextRequest>();
                TextFilters.Execute(textReq, middlareResult);
            }

            //处理图片消息
            if (middlareResult.Input.RequestBaseModel.MsgType == Configurations.Current.MessageType.Image)
            {
                var imageReq = middlareResult.Input.GetRequestModel<ImageRequest>();
                ImageFilters.Execute(imageReq, middlareResult);
            }

            //位置信息
            if (middlareResult.Input.RequestBaseModel.MsgType == Configurations.Current.MessageType.Location)
            {
                var locationReq = middlareResult.Input.GetRequestModel<LocationRequest>();
                LocationFilters.Execute(locationReq, middlareResult);
            }

            //事件
            if (middlareResult.Input.RequestBaseModel.MsgType == Configurations.Current.MessageType.Event)
            {
                var eb = middlareResult.Input.GetRequestModel<EventBase>();
                //普通过滤器
                EventFileters.Execute(eb, middlareResult);

                if (eb.Event == Configurations.Current.EventType.Click)
                {
                    //单击菜单事件
                    var clickReq = middlareResult.Input.GetRequestModel<ClickMenuRequest>();
                    ClickFilters.Execute(clickReq, middlareResult);
                }
                if (eb.Event == Configurations.Current.EventType.Scan)
                {
                    //扫描二维码事件
                    var scanReq = middlareResult.Input.GetRequestModel<ScanQrRequest>();
                    ScanQrFilters.Execute(scanReq, middlareResult);
                }
                if (eb.Event == Configurations.Current.EventType.Location)
                {
                    //位置事件
                    var positionReq = middlareResult.Input.GetRequestModel<PositionRequest>();
                    PositionFilter.Execute(positionReq, middlareResult);
                }
            }

            //全局结束过滤器
            GlobalEndFilters.ForEach(filter => filter(middlareResult));

            return middlareResult;
        }

        #endregion

    }
}

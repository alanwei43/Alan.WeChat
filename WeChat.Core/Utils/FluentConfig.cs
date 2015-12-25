﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.Core.Cache;
using WeChat.Core.Log;
using WeChat.Core.Messages;
using WeChat.Core.Messages.Events;
using WeChat.Core.Messages.Middlewares;
using WeChat.Core.Messages.Normal;

namespace WeChat.Core.Utils
{
    public static class MiddlewareInjectUtils
    {

    }
    /// <summary>
    /// Fluent配置辅助类
    /// </summary>
    public class FluentConfig
    {
        public static FluentConfig Get()
        {
            return new FluentConfig();
        }

        #region inject config, cache, log modules
        public FluentConfig Inject(string configFilePath)
        {
            var json = System.IO.File.ReadAllText(configFilePath);
            Configurations.Inject(json);
            return this;
        }

        /// <summary>
        /// 注入配置
        /// </summary>
        /// <param name="token"></param>
        /// <param name="aesKey"></param>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public FluentConfig Inject(string token, string aesKey, string appId, string appSecret)
        {
            Configurations.Inject(new Configurations()
            {
                Token = token,
                AesKey = aesKey,
                AppId = appId,
                AppSecret = appSecret
            });
            return this;
        }

        /// <summary>
        /// 缓存模块注入
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        public FluentConfig Inject(ICache cache)
        {
            CacheUtils.Inject(cache);
            return this;
        }

        /// <summary>
        /// 日志模块注入
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public FluentConfig Inject(ILog log)
        {
            LogUtils.Inject(log);
            return this;
        }
        #endregion




        #region inject filters

        /// <summary>
        /// 注册全局事件/消息过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FluentConfig Inject(Action<MiddlewareParameter> filter)
        {
            Middleware.InjectGlobalPreFilter(filter);
            return this;
        }

        /// <summary>
        /// 注册全局事件/消息过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FluentConfig InjectPre(Action<MiddlewareParameter> filter)
        {
            Middleware.InjectGlobalPreFilter(filter);
            return this;
        }

        /// <summary>
        /// 注册全局事件/消息过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FluentConfig InjectEnd(Action<MiddlewareParameter> filter)
        {
            Middleware.InjectGlobalEndFilter(filter);
            return this;
        }

        /// <summary>
        /// 注册图片消息过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FluentConfig Inject(Action<ImageRequest, MiddlewareParameter> filter)
        {
            return this.InjectImg(filter);
        }

        /// <summary>
        /// 注册图片消息过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FluentConfig InjectImg(Action<ImageRequest, MiddlewareParameter> filter)
        {
            Middleware.ImageFilters.Inject(filter);
            return this;
        }

        public FluentConfig InjectImg(Func<ImageRequest, bool> where, Func<ImageRequest, ResponseBase> setResponse)
        {
            return this.InjectImg((req, middleware) =>
            {
                if (where(req))
                    middleware.SetResponseModel(setResponse(req));
            });
        }

        /// <summary>
        /// 注册文本消息过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FluentConfig InjectTxt(Action<TextRequest, MiddlewareParameter> filter)
        {
            Middleware.TextFilters.Inject(filter);
            return this;
        }

        public FluentConfig InjectTxt(Func<TextRequest, bool> where, Func<TextRequest, ResponseBase> setResponse)
        {
            this.InjectTxt((req, middleware) =>
            {
                if (where(req))
                {
                    middleware.SetResponseModel(setResponse(req));
                }
            });
            return this;
        }

        /// <summary>
        /// 注册文本消息过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FluentConfig Inject(Action<TextRequest, MiddlewareParameter> filter)
        {
            return this.InjectTxt(filter);
        }
        /// <summary>
        /// 注册位置消息过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FluentConfig InjectLoc(Action<LocationRequest, MiddlewareParameter> filter)
        {
            Middleware.LocationFilters.Inject(filter);
            return this;
        }
        /// <summary>
        /// 注册位置消息过滤器
        /// </summary>
        /// <returns></returns>
        public FluentConfig InjectLoc(Func<LocationRequest, bool> where, Func<LocationRequest, ResponseBase> setResponse)
        {
            return this.InjectLoc((req, middleware) =>
            {
                if (where(req))
                    middleware.SetResponseModel(setResponse(req));
            });
        }
        /// <summary>
        /// 注册位置消息过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FluentConfig Inject(Action<LocationRequest, MiddlewareParameter> filter)
        {
            return this.InjectLoc(filter);
        }

        /// <summary>
        /// 注册单击事件过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FluentConfig InjectClick(Action<ClickMenuRequest, MiddlewareParameter> filter)
        {
            Middleware.ClickFilters.Inject(filter);
            return this;
        }
        /// <summary>
        /// 注册单击事件过滤器
        /// </summary>
        /// <returns></returns>
        public FluentConfig InjectClick(Func<ClickMenuRequest, bool> where, Func<ClickMenuRequest, ResponseBase> setResponse)
        {
            return this.InjectClick((req, middleware) =>
            {
                if (where(req))
                    middleware.SetResponseModel(setResponse(req));
            });
        }

        /// <summary>
        /// 注册单击事件过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FluentConfig Inject(Action<ClickMenuRequest, MiddlewareParameter> filter)
        {
            return this.InjectClick(filter);
        }

        /// <summary>
        /// 注册位置事件过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FluentConfig InjectPos(Action<PositionRequest, MiddlewareParameter> filter)
        {
            Middleware.PositionFilter.Inject(filter);
            return this;
        }

        /// <summary>
        /// 注册位置事件过滤器
        /// </summary>
        /// <returns></returns>
        public FluentConfig InjectPos(Func<PositionRequest, bool> where, Func<PositionRequest, ResponseBase> setResponse)
        {
            this.InjectPos((req, middleware) =>
            {
                if (where(req))
                {
                    middleware.SetResponseModel(setResponse(req));
                }
            });
            return this;
        }

        /// <summary>
        /// 注册位置事件过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FluentConfig Inject(Action<PositionRequest, MiddlewareParameter> filter)
        {
            return this.InjectPos(filter);
        }

        /// <summary>
        /// 注册扫描QR Code事件过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FluentConfig InjectScan(Action<ScanQrRequest, MiddlewareParameter> filter)
        {
            Middleware.ScanQrFilters.Inject(filter);
            return this;
        }
        /// <summary>
        /// 注册扫描QR Code事件过滤器
        /// </summary>
        /// <returns></returns>
        public FluentConfig InjectScan(Func<ScanQrRequest, bool> where, Func<ScanQrRequest, ResponseBase> setResponse)
        {
            return this.InjectScan((req, middleware) =>
            {
                if (where(req)) middleware.SetResponseModel(setResponse(req));
            });
        }
        /// <summary>
        /// 注册扫描QR Code事件过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FluentConfig Inject(Action<ScanQrRequest, MiddlewareParameter> filter)
        {
            return this.InjectScan(filter);
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.Core.Cache;
using WeChat.Core.Log;
using WeChat.Core.Messages.Events;
using WeChat.Core.Messages.Middlewares;
using WeChat.Core.Messages.Normal;

namespace WeChat.Core.Utils
{
    public static class MiddlewareInjectUtils
    {
        /// <summary>
        /// 注册全局事件/消息过滤器
        /// </summary>
        /// <param name="config"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FluentConfig Inject(this FluentConfig config, Action<MiddlewareParameter> filter)
        {
            Middleware.InjectGlobalPreFilter(filter);
            return config;
        }
        /// <summary>
        /// 注册全局事件/消息过滤器
        /// </summary>
        /// <param name="config"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FluentConfig InjectPre(this FluentConfig config, Action<MiddlewareParameter> filter)
        {
            Middleware.InjectGlobalPreFilter(filter);
            return config;
        }

        /// <summary>
        /// 注册全局事件/消息过滤器
        /// </summary>
        /// <param name="config"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FluentConfig InjectEnd(this FluentConfig config, Action<MiddlewareParameter> filter)
        {
            Middleware.InjectGlobalEndFilter(filter);
            return config;
        }

        /// <summary>
        /// 注册图片消息过滤器
        /// </summary>
        /// <param name="config"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FluentConfig Inject(this FluentConfig config, Action<ImageRequest, MiddlewareParameter> filter)
        {
            return config.InjectImg(filter);
        }
        /// <summary>
        /// 注册图片消息过滤器
        /// </summary>
        /// <param name="config"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FluentConfig InjectImg(this FluentConfig config, Action<ImageRequest, MiddlewareParameter> filter)
        {
            Middleware.ImageFilters.Inject(filter);
            return config;
        }

        /// <summary>
        /// 注册文本消息过滤器
        /// </summary>
        /// <param name="config"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FluentConfig InjectTxt(this FluentConfig config, Action<TextRequest, MiddlewareParameter> filter)
        {
            Middleware.TextFilters.Inject(filter);
            return config;
        }
        /// <summary>
        /// 注册文本消息过滤器
        /// </summary>
        /// <param name="config"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FluentConfig Inject(this FluentConfig config, Action<TextRequest, MiddlewareParameter> filter)
        {
            return config.InjectTxt(filter);
        }
        /// <summary>
        /// 注册位置消息过滤器
        /// </summary>
        /// <param name="config"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FluentConfig InjectLoc(this FluentConfig config, Action<LocationRequest, MiddlewareParameter> filter)
        {
            Middleware.LocationFilters.Inject(filter);
            return config;
        }
        /// <summary>
        /// 注册位置消息过滤器
        /// </summary>
        /// <param name="config"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FluentConfig Inject(this FluentConfig config, Action<LocationRequest, MiddlewareParameter> filter)
        {
            return config.InjectLoc(filter);
        }

        /// <summary>
        /// 注册单击事件过滤器
        /// </summary>
        /// <param name="config"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FluentConfig InjectClick(this FluentConfig config, Action<ClickMenuRequest, MiddlewareParameter> filter)
        {
            Middleware.ClickFilters.Inject(filter);
            return config;
        }

        /// <summary>
        /// 注册单击事件过滤器
        /// </summary>
        /// <param name="config"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FluentConfig Inject(this FluentConfig config, Action<ClickMenuRequest, MiddlewareParameter> filter)
        {
            return config.Inject(filter);
        }

        /// <summary>
        /// 注册位置事件过滤器
        /// </summary>
        /// <param name="config"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FluentConfig InjectPos(this FluentConfig config, Action<PositionRequest, MiddlewareParameter> filter)
        {
            Middleware.PositionFilter.Inject(filter);
            return config;
        }
        /// <summary>
        /// 注册位置事件过滤器
        /// </summary>
        /// <param name="config"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FluentConfig Inject(this FluentConfig config, Action<PositionRequest, MiddlewareParameter> filter)
        {
            return config.Inject(filter);
        }
        /// <summary>
        /// 注册扫描QR Code事件过滤器
        /// </summary>
        /// <param name="config"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FluentConfig InjectScan(this FluentConfig config, Action<ScanQrRequest, MiddlewareParameter> filter)
        {
            Middleware.ScanQrFilters.Inject(filter);
            return config;
        }
        /// <summary>
        /// 注册扫描QR Code事件过滤器
        /// </summary>
        /// <param name="config"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FluentConfig Inject(this FluentConfig config, Action<ScanQrRequest, MiddlewareParameter> filter)
        {
            return config.InjectScan(filter);
        }
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
    }
}

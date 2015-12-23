using System;
using System.Collections.Generic;

namespace WeChat.Core.Messages.Middlewares
{
    /// <summary>
    /// 过滤器容器
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public class FiltersContainer<TRequest>
    {
        /// <summary>
        /// 过滤器
        /// </summary>
        private readonly List<Action<TRequest, MiddlewareParameter>> _filters;

        public FiltersContainer()
        {
            _filters = new List<Action<TRequest, MiddlewareParameter>>();
        }

        public int Inject(Action<TRequest, MiddlewareParameter> filter)
        {
            _filters.Add(filter);
            return _filters.Count - 1;
        }

        public void Execute(TRequest req, MiddlewareParameter middleware)
        {
            this._filters.ForEach(filter => filter(req, middleware));
        }
    }

}

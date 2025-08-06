using System;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace JiaSyuanLibrary.NetFramework.Helper.AutoMapping
{
    /// <summary>
    /// 建立與管理 AutoMapper 的 IMapper 實例，支援快取與 loggerFactory 設定。
    /// </summary>
    public static class MapperFactory
    {
        private static readonly ConcurrentDictionary<string, Lazy<IMapper>> MapperCache
            = new ConcurrentDictionary<string, Lazy<IMapper>>();

        /// <summary>
        /// 根據指定的配置委派建立 IMapper 實例，並可選用 loggerFactory。
        /// </summary>
        public static IMapper GetMapper(Action<IMapperConfigurationExpression> configAction,
            string cacheKey = "default",
            ILoggerFactory loggerFactory = null)
        {
            if (configAction == null)
                throw new ArgumentNullException(nameof(configAction));

            var lazyMapper = MapperCache.GetOrAdd(cacheKey, _ => new Lazy<IMapper>(() =>
            {
                var cfgExp = new MapperConfigurationExpression();
                configAction.Invoke(cfgExp);

                var config = new MapperConfiguration(cfgExp, loggerFactory);
                config.AssertConfigurationIsValid();

                return new Mapper(config);
            }));

            return lazyMapper.Value;
        }

        /// <summary>
        /// 檢查指定快取鍵是否已建立對應的 Mapper。
        /// </summary>
        public static bool IsCached(string cacheKey) => MapperCache.ContainsKey(cacheKey);

        /// <summary>
        /// 清除所有快取中的 Mapper 實例。
        /// </summary>
        public static void ClearCache() => MapperCache.Clear();
    }

}

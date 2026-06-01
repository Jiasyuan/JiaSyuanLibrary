using System;
using System.Collections.Concurrent;
using Mapster;
using MapsterMapper;

namespace JiaSyuanLibrary.NetFramework.Helper.AutoMapping
{
    /// <summary>
    /// 建立與管理 AutoMapper 的 IMapper 實例，支援快取與 loggerFactory 設定。
    /// </summary>
    public static class MapperFactory
    {
        // 快取 Mapster 的 IMapper 實例
        private static readonly ConcurrentDictionary<string, Lazy<IMapper>> MapperCache
            = new ConcurrentDictionary<string, Lazy<IMapper>>();

        /// <summary>
        /// 根據指定的配置委派建立 IMapper 實例
        /// </summary>
        public static IMapper GetMapper(Action<TypeAdapterConfig> configAction, string cacheKey = "default")
        {
            if (configAction == null)
                throw new ArgumentNullException(nameof(configAction));

            var lazyMapper = MapperCache.GetOrAdd(cacheKey, _ => new Lazy<IMapper>(() =>
            {
                // 1. 建立獨立的 Mapster 配置設定
                var config = new TypeAdapterConfig();

                // 2. 注入外部規則
                configAction.Invoke(config);

                // 3. 預先編譯並驗證（等同於 AssertConfigurationIsValid）
                config.Compile();

                // 4. 回傳綁定此獨立配置的 Mapper 實例
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
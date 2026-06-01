using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Mapster;

namespace JiaSyuanLibrary.NetFramework.Helper.AutoMapping
{
    /// <summary>
    /// 管理命名的 Mapping Profile 註冊與查詢邏輯。
    /// </summary>
    public static class MappingProfileRegistry
    {
        // 將 Value 改為 Mapster 的 Action<TypeAdapterConfig>
        private static readonly ConcurrentDictionary<string, Action<TypeAdapterConfig>> ProfileMap
            = new ConcurrentDictionary<string, Action<TypeAdapterConfig>>();

        /// <summary>
        /// 預設 Profile 設定，用於 fallback。
        /// </summary>
        public static Action<TypeAdapterConfig> DefaultProfile => cfg =>
        {
            // Mapster 預設就是允許 Null 集合，這裡可以留空或做全域基本設定
        };

        /// <summary>
        /// 註冊指定名稱的 Profile 設定委派。
        /// </summary>
        public static void Register(string profileName, Action<TypeAdapterConfig> configAction)
        {
            if (string.IsNullOrWhiteSpace(profileName))
                throw new ArgumentException("Profile name cannot be null or empty", nameof(profileName));

            ProfileMap[profileName] = configAction ?? DefaultProfile;
        }

        /// <summary>
        /// 根據名稱取得已註冊的 Profile 設定，若不存在則返回 DefaultProfile。
        /// </summary>
        public static Action<TypeAdapterConfig> Get(string profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName) || !ProfileMap.TryGetValue(profileName, out var action))
                return DefaultProfile;

            return action;
        }

        /// <summary>
        /// 檢查指定名稱是否已註冊 Profile。
        /// </summary>
        public static bool IsRegistered(string profileName) => ProfileMap.ContainsKey(profileName);

        /// <summary>
        /// 列出所有已註冊 Profile 的名稱。
        /// </summary>
        public static IEnumerable<string> ListProfiles() => ProfileMap.Keys;
    }

}

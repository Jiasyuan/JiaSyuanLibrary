using AutoMapper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace JiaSyuanLibrary.Helper.AutoMapping
{
    /// <summary>
    /// 管理命名的 Mapping Profile 註冊與查詢邏輯。
    /// </summary>
    public static class MappingProfileRegistry
    {
        private static readonly ConcurrentDictionary<string, Action<IMapperConfigurationExpression>> ProfileMap
            = new ConcurrentDictionary<string, Action<IMapperConfigurationExpression>>();

        /// <summary>
        /// 預設 Profile 設定，用於 fallback。
        /// </summary>
        public static Action<IMapperConfigurationExpression> DefaultProfile => cfg =>
        {
            cfg.AllowNullCollections = true;
        };

        /// <summary>
        /// 註冊指定名稱的 Profile 設定委派。
        /// </summary>
        public static void Register(string profileName, Action<IMapperConfigurationExpression> configAction)
        {
            if (string.IsNullOrWhiteSpace(profileName))
                throw new ArgumentException("Profile name cannot be null or empty", nameof(profileName));

            ProfileMap[profileName] = configAction ?? DefaultProfile;
        }

        /// <summary>
        /// 根據名稱取得已註冊的 Profile 設定，若不存在則返回 DefaultProfile。
        /// </summary>
        public static Action<IMapperConfigurationExpression> Get(string profileName)
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
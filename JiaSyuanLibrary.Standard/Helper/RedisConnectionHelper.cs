using System;
using StackExchange.Redis;

namespace JiaSyuanLibrary.Standard.Helper
{
    /// <summary>
    /// RedisConnectionHelper
    /// </summary>
    public sealed class RedisConnectionHelper
    {
        public readonly ConnectionMultiplexer ConnectionMultiplexer;
        private static string _settingOption;
        private RedisConnectionHelper()
        {
            ConnectionMultiplexer = ConnectionMultiplexer.Connect(_settingOption);
        }

        public static void Init(string settingOption)
        {
            _settingOption = settingOption;
        }

        public static RedisConnectionHelper Instance => lazy.Value;


        private static Lazy<RedisConnectionHelper> lazy = new Lazy<RedisConnectionHelper>(() =>
        {
            if (string.IsNullOrEmpty(_settingOption)) throw new InvalidOperationException("Please call Init() first.");
            return new RedisConnectionHelper();
        });
    }
}

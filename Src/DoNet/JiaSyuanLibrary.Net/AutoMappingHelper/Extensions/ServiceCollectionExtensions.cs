using JiaSyuanLibrary.Net.AutoMappingHelper.Core;
using JiaSyuanLibrary.Net.AutoMappingHelper.Interface;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JiaSyuanLibrary.Net.AutoMappingHelper.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMapsterWithProfiles(
            this IServiceCollection services,
            Action<IProfileRegistry> registerProfiles,
            ILoggerFactory? loggerFactory = null)
        {
            var registry = new ProfileRegistry();
            registerProfiles(registry);

            // 建立 Mapster 的全域配置
            var config = new TypeAdapterConfig();

            // 執行所有模組註冊的對映規則
            foreach (var profileAction in registry.GetAllProfiles())
            {
                profileAction(config);
            }

            try
            {
                // 讓 Mapster 預先編譯並驗證對映關係（類似 AssertConfigurationIsValid）
                config.Compile();
            }
            catch (Exception ex)
            {
                loggerFactory?.CreateLogger("Mapster")?
                    .LogError(ex, "Mapster configuration compilation failed.");
                throw;
            }

            // 註冊成 Mapster 官方支援的 DI 模式
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>(); // Mapster 官方推薦用 Scoped 生命週期處理 IMapper
            services.AddScoped<AutoMappingHelper>();     // 配合 IMapper 改為 Scoped

            services.AddSingleton<IProfileRegistry>(registry);

            return services;
        }
    }
}
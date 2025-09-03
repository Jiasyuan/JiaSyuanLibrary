using AutoMapper;
using JiaSyuanLibrary.Net.AutoMappingHelper.Core;
using JiaSyuanLibrary.Net.AutoMappingHelper.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JiaSyuanLibrary.Net.AutoMappingHelper.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapperWithProfiles(
            this IServiceCollection services,
            Action<IProfileRegistry> registerProfiles,
            ILoggerFactory? loggerFactory = null)
        {
            var registry = new ProfileRegistry();
            registerProfiles(registry);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                foreach (var profileAction in registry.GetAllProfiles())
                {
                    profileAction(cfg);
                }
            }, loggerFactory);

            try
            {
                mapperConfig.AssertConfigurationIsValid();
            }
            catch (Exception ex)
            {
                loggerFactory?.CreateLogger("AutoMapper")?
                    .LogError(ex, "AutoMapper configuration validation failed.");
                throw;
            }

            var mapper = mapperConfig.CreateMapper();

            services.AddSingleton<IProfileRegistry>(registry);
            services.AddSingleton<IMapper>(mapper);
            services.AddSingleton<AutoMappingHelper>(); //註冊 AutoMappingHelper

            return services;
        }
    }
}
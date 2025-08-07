using AutoMapper;
using JiaSyuanLibrary.Net.AutoMappingHelper.Core;
using JiaSyuanLibrary.Net.AutoMappingHelper.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JiaSyuanLibrary.Net.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapperWithProfiles(
            this IServiceCollection services,
            Action<IProfileRegistry> registerAction,
            ILoggerFactory? loggerFactory = null)
        {
            var registry = new ProfileRegistry();
            registerAction.Invoke(registry);

            var configExp = new MapperConfigurationExpression();
            foreach (var profile in registry.GetAllProfiles())
            {
                profile.Invoke(configExp);
            }

            var config = new MapperConfiguration(configExp, loggerFactory);

            config.AssertConfigurationIsValid();

            IMapper mapper = new Mapper(config);
            services.AddSingleton(mapper);

            return services;
        }
    }

}

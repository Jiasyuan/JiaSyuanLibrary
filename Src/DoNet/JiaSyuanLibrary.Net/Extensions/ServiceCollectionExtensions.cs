using AutoMapper;
using JiaSyuanLibrary.Net.AutoMappingHelper;
using JiaSyuanLibrary.Net.AutoMappingHelper.Core;
using JiaSyuanLibrary.Net.AutoMappingHelper.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

        var mapper = mapperConfig.CreateMapper();

        services.AddSingleton<IProfileRegistry>(registry);
        services.AddSingleton<IMapper>(mapper);
        services.AddSingleton<AutoMappingHelper>(); //註冊 AutoMappingHelper

        return services;
    }
}
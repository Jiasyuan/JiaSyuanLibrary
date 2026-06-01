using JiaSyuanLibrary.Net.AutoMappingHelper.Interface;
using Microsoft.Extensions.Logging;

namespace JiaSyuanLibrary.Net.AutoMappingHelper.Modules
{
    public static class MapperModules
    {
        public static void RegisterModulesAuto(IProfileRegistry registry, ILogger? logger = null)
        {
            var moduleTypes = typeof(IMappingProfileModule).Assembly.GetTypes()
                .Where(t => typeof(IMappingProfileModule).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var type in moduleTypes)
            {
                try
                {
                    if (Activator.CreateInstance(type) is IMappingProfileModule module)
                    {
                        logger?.LogInformation("Auto-registering AutoMapper module: {ModuleName}", type.Name);
                        module.Register(registry);
                    }
                    else
                    {
                        logger?.LogWarning("Failed to instantiate AutoMapper module: {TypeName}", type.FullName);
                    }
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, "Exception occurred while registering AutoMapper module: {TypeName}", type.FullName);
                }
            }
        }
    }
}
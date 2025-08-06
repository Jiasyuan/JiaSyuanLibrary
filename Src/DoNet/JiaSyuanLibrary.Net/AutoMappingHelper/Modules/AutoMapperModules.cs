using JiaSyuanLibrary.Net.AutoMappingHelper.Interface;

namespace JiaSyuanLibrary.Net.AutoMappingHelper.Modules
{
    public static class AutoMapperModules
    {
        public static void RegisterModules(IProfileRegistry registry)
        {
            var modules = new List<IMappingProfileModule>
            {
                //new UserProfileModule(),
                //new ProductProfileModule()
            };

            foreach (var module in modules)
                module.Register(registry);
        }

        public static void RegisterModulesAuto(IProfileRegistry registry)
        {
            var moduleTypes = typeof(IMappingProfileModule).Assembly.GetTypes()
                .Where(t => typeof(IMappingProfileModule).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var type in moduleTypes)
            {
                if (Activator.CreateInstance(type) is IMappingProfileModule module)
                    module.Register(registry);
            }
        }
    }

}

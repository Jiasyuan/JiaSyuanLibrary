using AutoMapper;
using JiaSyuanLibrary.Net.AutoMappingHelper.Interface;

namespace JiaSyuanLibrary.Net.AutoMappingHelper.Core
{
    public class ProfileRegistry : IProfileRegistry
    {
        private readonly List<Action<IMapperConfigurationExpression>> profiles = new();

        public void Register(string profileName, Action<IMapperConfigurationExpression> configAction)
        {
            profiles.Add(configAction ?? (_ => { }));
        }

        public IEnumerable<Action<IMapperConfigurationExpression>> GetAllProfiles() => profiles;
    }

}

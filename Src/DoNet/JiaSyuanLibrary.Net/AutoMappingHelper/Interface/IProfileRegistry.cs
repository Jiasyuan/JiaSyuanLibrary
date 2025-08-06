using AutoMapper;

namespace JiaSyuanLibrary.Net.AutoMappingHelper.Interface
{
    public interface IProfileRegistry
    {
        void Register(string profileName, Action<IMapperConfigurationExpression> configAction);
        IEnumerable<Action<IMapperConfigurationExpression>> GetAllProfiles();
    }

}
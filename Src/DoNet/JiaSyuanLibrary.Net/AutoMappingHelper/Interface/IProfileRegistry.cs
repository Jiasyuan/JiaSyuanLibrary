using AutoMapper;
using Microsoft.Extensions.Logging;

namespace JiaSyuanLibrary.Net.AutoMappingHelper.Interface
{
    public interface IProfileRegistry
    {
        void Register(string profileName, Action<IMapperConfigurationExpression> configAction, ILogger? logger = null);
        IEnumerable<Action<IMapperConfigurationExpression>> GetAllProfiles();
        IEnumerable<string> GetAllProfileNames();
        bool ContainsProfile(string profileName);
    }
}
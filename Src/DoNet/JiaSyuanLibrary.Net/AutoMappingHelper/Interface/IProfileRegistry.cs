using Mapster;
using Microsoft.Extensions.Logging;

namespace JiaSyuanLibrary.Net.AutoMappingHelper.Interface
{
    public interface IProfileRegistry
    {
        // 將 Action 內容換成 Mapster 的 TypeAdapterConfig
        void Register(string profileName, Action<TypeAdapterConfig> configAction, ILogger? logger = null);
        IEnumerable<Action<TypeAdapterConfig>> GetAllProfiles();
        IEnumerable<string> GetAllProfileNames();
        bool ContainsProfile(string profileName);
    }
}
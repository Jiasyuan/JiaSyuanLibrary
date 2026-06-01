using JiaSyuanLibrary.Net.AutoMappingHelper.Interface;
using Mapster;
using Microsoft.Extensions.Logging;

namespace JiaSyuanLibrary.Net.AutoMappingHelper.Core
{
    public class ProfileRegistry : IProfileRegistry
    {
        private readonly Dictionary<string, Action<TypeAdapterConfig>> _profiles = new();

        public void Register(string profileName, Action<TypeAdapterConfig> configAction, ILogger? logger = null)
        {
            if (string.IsNullOrWhiteSpace(profileName))
                throw new ArgumentException("Profile name cannot be null or whitespace.", nameof(profileName));

            if (_profiles.ContainsKey(profileName))
                logger?.LogWarning("Profile '{ProfileName}' is being overwritten.", profileName);

            _profiles[profileName] = configAction;
        }

        public IEnumerable<Action<TypeAdapterConfig>> GetAllProfiles() => _profiles.Values;
        public IEnumerable<string> GetAllProfileNames() => _profiles.Keys;
        public bool ContainsProfile(string profileName) => _profiles.ContainsKey(profileName);
    }
}

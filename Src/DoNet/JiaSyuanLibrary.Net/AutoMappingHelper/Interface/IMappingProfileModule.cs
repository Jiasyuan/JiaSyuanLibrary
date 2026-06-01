using Microsoft.Extensions.Logging;

namespace JiaSyuanLibrary.Net.AutoMappingHelper.Interface
{
    public interface IMappingProfileModule
    {
        void Register(IProfileRegistry registry, ILogger? logger = null);
    }
}

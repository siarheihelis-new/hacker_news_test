using Microsoft.Extensions.Configuration;

namespace HackerNews.Core.Modularity.Impl
{
    internal class ConfigurationModuleProvider : IModuleProvider
    {
        private readonly IConfiguration _configuration;

        public ConfigurationModuleProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<ModuleInfo> GetModulesBySection(string sectionName)
        {
            return _configuration.GetSection(sectionName).Get<IEnumerable<ModuleInfo>>() ??
                 new List<ModuleInfo>();
        }
    }
}

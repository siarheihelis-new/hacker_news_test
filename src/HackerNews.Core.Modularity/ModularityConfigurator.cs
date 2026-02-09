using HackerNews.Core.Modularity.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HackerNews.Core.Modularity
{
    public class ModularityConfigurator : IConfigurator
    {
        public IServiceCollection Configure(IServiceCollection services)
        {
            services.TryAddSingleton<IModuleProvider, ConfigurationModuleProvider>();
            services.TryAddSingleton<IModuleManager, ModuleManager>();
            return services;
        }
    }
}

using HackerNews.Core.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNews.Api
{
    /// <summary>
    /// Represents the API module for best stories.
    /// </summary>
    public class Module : IModule
    {
        public string Name => "Best stories";

        public IServiceCollection Configure(IServiceCollection services)
        {
            //Configure business logic dependencies
            IConfigurator logicConfigurator = new Logic.Configurator();
            logicConfigurator.Configure(services);
            return services;
        }
    }
}

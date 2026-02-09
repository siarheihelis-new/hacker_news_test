using Microsoft.Extensions.DependencyInjection;
using HackerNews.Core.Modularity;
using HackerNews.Logic.Impl;
using HackerNews.Data;

namespace HackerNews.Logic
{
    public class Configurator : IConfigurator
    {
        public IServiceCollection Configure(IServiceCollection services)
        {
            //configure data dependencies
            IConfigurator dataConfigurator = new Data.Configurator();
            dataConfigurator.Configure(services);
            services.Decorate<IHackerNewsApiClient, CachedHackerNewsApiClient>(); ;
            services.AddScoped<IBestStoriesService, BestStoriesService>();
            return services;
        }
    }
}

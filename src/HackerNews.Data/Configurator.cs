using HackerNews.Core.Modularity;
using HackerNews.Data.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNews.Data
{
    public class Configurator : IConfigurator
    {
        public IServiceCollection Configure(IServiceCollection services)
        {
            services.AddHttpClient<IHackerNewsApiClient, HackerNewsApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
                client.Timeout = TimeSpan.FromSeconds(10);
            })
            .AddResilience(); ;
            return services;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace HackerNews.Core.Caching
{
    public static class CachingExtensions
    {
        public static IServiceCollection AddObjectCache(this IServiceCollection services)
        {
            //TODO: we can add other cache implementations like Redis, etc. and make it configurable
            services.AddMemoryCache();
            services.AddSingleton<IObjectCache, MemoryObjectCache>();
            return services;
        }
    }
}

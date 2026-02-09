using Microsoft.Extensions.Caching.Memory;

namespace HackerNews.Core.Caching
{
    internal class MemoryObjectCache : IObjectCache
    {
        private readonly IMemoryCache _cache;
        public MemoryObjectCache(IMemoryCache cache)
        {
            _cache = cache;
        }
        public Task<T?> GetAsync<T>(string key) where T : class
        {
            return Task.FromResult(_cache.Get<T>(key));
        }
        public Task SetAsync<T>(string key, T value, TimeSpan ttl) where T : class
        {
            _cache.Set(key, value, ttl);
            return Task.CompletedTask;
        }
    }
}

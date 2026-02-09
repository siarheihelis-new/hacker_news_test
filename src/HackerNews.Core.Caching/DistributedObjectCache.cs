using Microsoft.Extensions.Caching.Distributed;

namespace HackerNews.Core.Caching
{
    public class DistributedObjectCache : IObjectCache
    {
        private readonly IDistributedCache _cache;
        public DistributedObjectCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            var json = await _cache.GetStringAsync(key);
            return json == null
                ? default
                : System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan ttl) where T : class
        {
            var json = System.Text.Json.JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(
                key,
                json,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = ttl
                });
        }
    }
}

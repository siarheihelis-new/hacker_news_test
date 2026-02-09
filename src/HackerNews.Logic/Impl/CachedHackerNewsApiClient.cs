using HackerNews.Core.Caching;
using HackerNews.Data;
using HackerNews.Data.Entity;

namespace HackerNews.Logic.Impl
{
    internal class CachedHackerNewsApiClient : IHackerNewsApiClient
    {
        private readonly IHackerNewsApiClient _innerClient;
        private readonly IObjectCache _cache;

        public CachedHackerNewsApiClient(IHackerNewsApiClient innerClient, IObjectCache cache)
        {
            _innerClient = innerClient;
            _cache = cache;
        }

        public async Task<IReadOnlyList<long>> GetBestStoriesIdsAsync()
        {
            const string cacheKey = "beststories";

            var cached = await _cache.GetAsync<List<long>>(cacheKey);
            if (cached != null)
            {
                return cached;
            }

            var ids = await _innerClient.GetBestStoriesIdsAsync();

            await _cache.SetAsync(cacheKey, ids, TimeSpan.FromMinutes(1));

            return ids;
        }

        public async Task<HackerNewsItem?> GetStoryAsync(long id)
        {
            var cacheKey = $"story:{id}";

            var cached = await _cache.GetAsync<HackerNewsItem>(cacheKey);
            if (cached != null)
                return cached;

            var item = await _innerClient.GetStoryAsync(id);

            if (item != null)
                await _cache.SetAsync(cacheKey, item, TimeSpan.FromMinutes(5));

            return item;
        }
    }
}

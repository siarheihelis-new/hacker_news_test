using HackerNews.Contract;
using HackerNews.Core.Caching;
using HackerNews.Data;

namespace HackerNews.Logic.Impl
{
    public class BestStoriesService : IBestStoriesService
    {
        private readonly IHackerNewsApiClient _client;
        private readonly IObjectCache _cache;
        private const int MaxConcurrentRequests = 10;
        private readonly SemaphoreSlim _throttle = new SemaphoreSlim(MaxConcurrentRequests);

        public BestStoriesService(IHackerNewsApiClient client, IObjectCache cache)
        {
            _client = client;
            _cache = cache;
        }

        public async Task<IEnumerable<Story>> GetBestStoriesAsync(int count)
        {
            var ids = await _client.GetBestStoriesIdsAsync();

            var selectedIds = ids.Take(count);

            var tasks = selectedIds.Select(async id =>
            {
                await _throttle.WaitAsync();
                try
                {
                    var item = await _client.GetStoryAsync(id);

                    if (item == null) return null;

                    return new Story
                    {
                        Title = item.Title,
                        Uri = item.Url,
                        PostedBy = item.By,
                        Time = DateTimeOffset.FromUnixTimeSeconds(item.Time).UtcDateTime,
                        Score = item.Score,
                        CommentCount = item.Descendants
                    };
                }
                finally
                {
                    _throttle.Release();
                }
            });

            var stories = (await Task.WhenAll(tasks))
                .Where(x => x != null)
                .OrderByDescending(x => x!.Score)
                .ToList()!;

            return stories;
        }        
    }
}

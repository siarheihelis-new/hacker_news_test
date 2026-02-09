using HackerNews.Data.Entity;

namespace HackerNews.Data
{
    public interface IHackerNewsApiClient
    {
        Task<IReadOnlyList<long>> GetBestStoriesIdsAsync();
        Task<HackerNewsItem?> GetStoryAsync(long id);
    }
}

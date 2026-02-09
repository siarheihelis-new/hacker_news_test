namespace HackerNews.Core.Caching
{
    public interface IObjectCache
    {
        Task<T?> GetAsync<T>(string key) where T : class;
        Task SetAsync<T>(string key, T value, TimeSpan ttl) where T : class;

    }
}

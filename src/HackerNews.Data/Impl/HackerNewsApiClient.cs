using HackerNews.Data.Entity;
using System.Net.Http.Json;

namespace HackerNews.Data.Impl
{
    internal class HackerNewsApiClient : IHackerNewsApiClient
    {
        private readonly HttpClient _httpClient;

        public HackerNewsApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<long>> GetBestStoriesIdsAsync()
        {
            return await _httpClient
                .GetFromJsonAsync<List<long>>("beststories.json")
                ?? new List<long>();
        }

        public async Task<HackerNewsItem?> GetStoryAsync(long id)
        {
            return await _httpClient
                .GetFromJsonAsync<HackerNewsItem>($"item/{id}.json");
        }
    }
}

using HackerNews.Contract;

namespace HackerNews.Logic
{
    public interface IBestStoriesService
    {
        Task<IEnumerable<Story>> GetBestStoriesAsync(int count);
    }
}

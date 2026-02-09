using Microsoft.Extensions.DependencyInjection;

namespace HackerNews.Core.Modularity
{
    public interface IModule
    {
        string Name { get; }
        IServiceCollection Configure(IServiceCollection services);
    }
}

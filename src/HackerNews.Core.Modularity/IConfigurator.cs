using Microsoft.Extensions.DependencyInjection;

namespace HackerNews.Core.Modularity
{
    public interface IConfigurator
    {
        IServiceCollection Configure(IServiceCollection services);
    }
}

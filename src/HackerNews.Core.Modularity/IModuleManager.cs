using Microsoft.Extensions.DependencyInjection;

namespace HackerNews.Core.Modularity
{
    public interface IModuleManager
    {
        IServiceCollection Configure(string scope, IServiceCollection services);
        IList<IModule> GetModules(string scope);
    }
}

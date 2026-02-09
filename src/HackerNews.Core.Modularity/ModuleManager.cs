using HackerNews.Core.Modularity.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNews.Core.Modularity
{
    /// <summary>
    /// Implementation of <see cref="IModuleManager"/>
    /// </summary>
    internal class ModuleManager : IModuleManager
    {
        /// <summary>
        /// Holds <see cref="IModuleProvider"/> instance
        /// </summary>
        private readonly IModuleProvider _moduleProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, List<IModule>> _scopeModules = new Dictionary<string, List<IModule>>();

        /// <summary>
        /// Constructor for <see cref="ModuleManager"/>
        /// </summary>
        /// <param name="moduleProvider">module provider</param>
        /// <param name="serviceProvider">service provider</param>
        public ModuleManager(IModuleProvider moduleProvider, IServiceProvider serviceProvider)
        {
            _moduleProvider = moduleProvider;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Initializes modules.
        /// We can define scope for modules to use modules functionality as plug-ins for several places
        /// For example, scope equal to 'shell' will means that this plugin especially for shell
        /// </summary>
        /// <param name="scopeName">Name of modules to initialize</param>
        /// <param name="services">service collection</param>
        public IServiceCollection Configure(string scopeName, IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(scopeName, nameof(scopeName));
            _scopeModules.Add(scopeName, new List<IModule>());
            foreach (ModuleInfo moduleInfo in _moduleProvider.GetModulesBySection(scopeName))
            {
                if (!string.IsNullOrEmpty(moduleInfo.Type))
                {
                    Type? type = Type.GetType(moduleInfo.Type);

                    if (type != null)
                    {
                        if (ActivatorUtilities.CreateInstance(_serviceProvider, type) is IModule module)
                        {
                            services.AddSingleton(module);
                            _scopeModules[scopeName].Add(module);
                        }
                    }
                    else
                    {
                        //Logger.Error($"Invalid module type {moduleInfo}.");
                    }
                }
            }
            _scopeModules[scopeName].ForEach(m => m.Configure(services));
            return services;
        }

        /// <summary>
        /// Gets loaded modules
        /// </summary>
        public IList<IModule> GetModules(string scopeName)
        {
            ArgumentNullException.ThrowIfNull(scopeName, nameof(scopeName));
            if (_scopeModules.TryGetValue(scopeName, out var modules))
            {
                return modules;
            }
            return new List<IModule>();
        }
    }
}

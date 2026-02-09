using HackerNews.Core.Modularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNews.Core.AspNet
{
    /// <summary>
    /// Provides extension methods to integrate the modularity subsystem with ASP.NET Core.
    /// </summary>
    public static class ModularityExtensions
    {
        /// <summary>
        /// Logical scope name used to register and resolve modules that contribute API controllers.
        /// Modules configured under this scope will have their assemblies added as MVC application parts.
        /// </summary>
        private const string ApiScope = "ApiModules";

        /// <summary>
        /// Scans and registers API modules with the provided <see cref="IServiceCollection"/>, then
        /// adds the controller application parts for each discovered module's assembly.
        /// </summary>
        /// <param name="services">The services collection to configure and to which controllers will be added.</param>
        /// <returns>
        /// An <see cref="IMvcBuilder"/> configured with the application parts from discovered API modules.
        /// This allows further MVC configuration chaining in the host application's startup code.
        /// </returns>
        /// <remarks>
        /// This method:
        /// - Instantiates a <see cref="ModularityConfigurator"/> and configures the DI container for modularity.
        /// - Builds a temporary service provider to resolve <see cref="IModuleManager"/>.
        /// - Calls <c>moduleManager.Configure(ApiScope, services)</c> to allow modules to register services for the API scope.
        /// - Adds controllers and registers each module's assembly as an MVC application part so module controllers are discovered.
        /// </remarks>
        public static IMvcBuilder AddApiModules(this IServiceCollection services)
        {
            var configurator = new ModularityConfigurator();
            var moduleManager = configurator.Configure(services)
                .BuildServiceProvider()
                .GetRequiredService<IModuleManager>();
            moduleManager.Configure(ApiScope, services);

            var builder = services.AddControllers();
            foreach (var module in moduleManager.GetModules(ApiScope))
            {
                builder.AddApplicationPart(module.GetType().Assembly);
            }
            return builder;
        }

        /// <summary>
        /// Performs runtime initialization for API modules when the application pipeline is built.
        /// </summary>
        /// <param name="app">The application builder used to access application services.</param>
        /// <remarks>
        /// Currently the method resolves the <see cref="IModuleManager"/> from the application's service provider.
        /// Implementations may use the resolved manager to perform module-level initialization that requires
        /// the fully built application services (for example, starting background services, registering routes,
        /// or performing health checks).
        /// </remarks>
        public static void UseApiModules(this IApplicationBuilder app)
        {
            var moduleManager = app.ApplicationServices.GetRequiredService<IModuleManager>();
        }
    }
}

using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace HackerNews.Core.AspNet
{
    /// <summary>
    /// Extensions for <see cref="IConfigurationBuilder"/>
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Adds the JSON configuration provider at <paramref name="searchPattern"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="searchPattern">Search pattern to find files relative to the base path stored in
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddJsonFiles(this IConfigurationBuilder builder, string searchPattern, bool optional, bool reloadOnChange)
        {
            string? baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (baseDirectory != null)
            {
                foreach (var path in Directory.EnumerateFiles(baseDirectory, searchPattern))
                {
                    builder = builder.AddJsonFile(path, optional, reloadOnChange);
                }
            }
            return builder;
        }

        /// <summary>
        /// Adds configuration from JSON files that located within Configuration folder
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddJsonConfiguration(this IConfigurationBuilder builder)
        {
            return builder.AddJsonFiles("Configuration/*.json", false, true);
        }
    }
}

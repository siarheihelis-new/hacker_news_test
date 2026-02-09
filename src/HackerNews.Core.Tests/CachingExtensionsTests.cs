using HackerNews.Core.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNews.Core.Tests
{
    [TestClass]
    public class CachingExtensionsTests
    {
        [TestMethod]
        public void AddObjectCache_RegistersMemoryCache()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddObjectCache();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var cache = serviceProvider.GetRequiredService<IObjectCache>();
            Assert.IsNotNull(cache);
        }

        [TestMethod]
        public void AddObjectCache_RegistersMemoryCacheFactory()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddObjectCache();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
            Assert.IsNotNull(memoryCache);
        }

        [TestMethod]
        public void AddObjectCache_CanBeCalledMultipleTimes()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddObjectCache();
            services.AddObjectCache();

            // Assert - should not throw
            var serviceProvider = services.BuildServiceProvider();
            var cache = serviceProvider.GetRequiredService<IObjectCache>();
            Assert.IsNotNull(cache);
        }

    }
}

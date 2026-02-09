using HackerNews.Core.AspNet;
using Microsoft.Extensions.Configuration;

namespace HackerNews.Core.Tests
{
    [TestClass]
    public class ConfigurationExtensionsTests
    {
        [TestMethod]
        public void AddJsonFiles_AllowsChaining()
        {
            // Arrange
            var configBuilder = new ConfigurationBuilder();

            // Act & Assert
            var result = configBuilder
                .AddJsonFiles("config1.json", optional: true, reloadOnChange: false)
                .AddJsonFiles("config2.json", optional: true, reloadOnChange: false);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddJsonFiles_WithOptionalTrue_DoesNotThrowIfFilesNotFound()
        {
            // Arrange
            var configBuilder = new ConfigurationBuilder();

            // Act & Assert - should not throw
            configBuilder.AddJsonFiles("non-existent-*.json", optional: true, reloadOnChange: false);
            var config = configBuilder.Build();
            Assert.IsNotNull(config);
        }

    }
}

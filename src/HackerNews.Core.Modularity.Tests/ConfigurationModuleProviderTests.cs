using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using HackerNews.Core.Modularity.Impl;
using System.Collections.Generic;

namespace HackerNews.Core.Modularity.Tests
{
    [TestClass]
    public class ConfigurationModuleProviderTests
    {
        [TestMethod]
        public void GetModulesBySection_WhenSectionHasModules_ReturnsList()
        {
            // Arrange
            var inMemory = new Dictionary<string, string>
            {
                { "myScope:0:Type", "Some.Type, Some.Assembly" },
                { "myScope:0:Name", "Name1" },
                { "myScope:1:Type", "Other.Type, Other.Assembly" },
                { "myScope:1:Name", "Name2" }
            };
            var config = new ConfigurationBuilder().AddInMemoryCollection(inMemory).Build();
            var provider = new ConfigurationModuleProvider(config);

            // Act
            var modules = provider.GetModulesBySection("myScope");

            // Assert
            Assert.IsNotNull(modules);
            CollectionAssert.AreEqual(new[] { "Name1", "Name2" }, new List<string>(System.Linq.Enumerable.Select(modules, m => m.Name)));
        }

        [TestMethod]
        public void GetModulesBySection_WhenSectionMissing_ReturnsEmpty()
        {
            // Arrange
            var config = new ConfigurationBuilder().Build();
            var provider = new ConfigurationModuleProvider(config);

            // Act
            var modules = provider.GetModulesBySection("missing");

            // Assert
            Assert.IsNotNull(modules);
            Assert.AreEqual(0, System.Linq.Enumerable.Count(modules));
        }
    }
}

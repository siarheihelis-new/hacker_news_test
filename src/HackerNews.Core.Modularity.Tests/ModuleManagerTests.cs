using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using HackerNews.Core.Modularity;
using HackerNews.Core.Modularity.Impl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;

namespace HackerNews.Core.Modularity.Tests
{
    [TestClass]
    public class ModuleManagerTests
    {
        private class DummyMarker { }

        private class TestModule : IModule
        {
            public string Name => "TestModule";

            public IServiceCollection Configure(IServiceCollection services)
            {
                services.AddSingleton<DummyMarker>();
                return services;
            }
        }

        private ModuleManager CreateManagerWithModules(IEnumerable<ModuleInfo>? modules = null)
        {
            var providerMock = new Mock<IModuleProvider>();
            providerMock
                .Setup(p => p.GetModulesBySection(It.IsAny<string>()))
                .Returns((string _) => modules ?? Enumerable.Empty<ModuleInfo>());

            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var manager = new ModuleManager(providerMock.Object, serviceProvider);
            return manager;
        }

        [TestMethod]
        public void Configure_ShouldLoadModules_InvokeConfigure_AndRegisterServices()
        {
            // Arrange
            var moduleInfo = new ModuleInfo
            {
                Type = typeof(TestModule).AssemblyQualifiedName!,
                Name = "Test"
            };

            var manager = CreateManagerWithModules(new[] { moduleInfo });
            var services = new ServiceCollection();

            // Act
            manager.Configure("scope", services);

            // Assert - marker service added by TestModule.Configure
            Assert.IsTrue(services.Any(d => d.ServiceType == typeof(DummyMarker)), "Marker service was not registered.");

            var modules = manager.GetModules("scope");
            Assert.AreEqual(1, modules.Count);
            Assert.AreEqual("TestModule", modules[0].Name);
        }

        [TestMethod]
        public void GetModules_UnknownScope_ReturnsEmptyList()
        {
            // Arrange
            var manager = CreateManagerWithModules();

            // Act
            var modules = manager.GetModules("unknown");

            // Assert
            Assert.IsNotNull(modules);
            Assert.AreEqual(0, modules.Count);
        }

        [TestMethod]
        public void Configure_NullScope_ThrowsArgumentNullException()
        {
            // Arrange
            var manager = CreateManagerWithModules();

            // Act & Assert
            try
            {
                manager.Configure(null!, new ServiceCollection());
                Assert.Fail("Expected ArgumentNullException was not thrown.");
            }
            catch (ArgumentNullException)
            {
                // expected
            }
        }

        [TestMethod]
        public void GetModules_NullScope_ThrowsArgumentNullException()
        {
            // Arrange
            var manager = CreateManagerWithModules();

            // Act & Assert
            try
            {
                manager.GetModules(null!);
                Assert.Fail("Expected ArgumentNullException was not thrown.");
            }
            catch (ArgumentNullException)
            {
                // expected
            }
        }
    }
}

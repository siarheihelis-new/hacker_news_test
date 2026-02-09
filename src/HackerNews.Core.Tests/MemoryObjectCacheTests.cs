using HackerNews.Core.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNews.Core.Tests
{
    [TestClass]
    public class MemoryObjectCacheTests
    {
        private IObjectCache _cache = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            var services = new ServiceCollection();
            services.AddObjectCache();
            var serviceProvider = services.BuildServiceProvider();
            _cache = serviceProvider.GetRequiredService<IObjectCache>();
        }


        [TestCleanup]
        public void TestCleanup()
        {
            // Cleanup if needed
        }

        [TestMethod]
        public async Task GetAsync_WithExistingKey_ReturnsValue()
        {
            // Arrange
            var key = "test-key";
            var testValue = new TestObject { Id = 1, Name = "Test" };
            // Pre-populate the cache by setting a value
            await _cache.SetAsync(key, testValue, TimeSpan.FromMinutes(10));

            // Act
            var result = await _cache.GetAsync<TestObject>(key);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testValue.Id, result.Id);
            Assert.AreEqual(testValue.Name, result.Name);
        }

        [TestMethod]
        public async Task GetAsync_WithNonExistentKey_ReturnsNull()
        {
            // Arrange
            var key = "non-existent-key";

            // Act
            var result = await _cache.GetAsync<TestObject>(key);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task SetAsync_SetsValueInCache()
        {
            // Arrange
            var key = "test-key";
            var testValue = new TestObject { Id = 1, Name = "Test" };
            var ttl = TimeSpan.FromMinutes(5);

            // Act
            await _cache.SetAsync(key, testValue, ttl);
            var result = await _cache.GetAsync<TestObject>(key);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testValue.Id, result.Id);
            Assert.AreEqual(testValue.Name, result.Name);
        }

        [TestMethod]
        public async Task SetAsync_WithDifferentTtl_ExpiresCorrectly()
        {
            // Arrange
            var key = "expiring-key";
            var testValue = new TestObject { Id = 2, Name = "Expiring" };
            var ttl = TimeSpan.FromMilliseconds(100);

            // Act
            await _cache.SetAsync(key, testValue, ttl);
            var resultBefore = await _cache.GetAsync<TestObject>(key);

            // Wait for expiration
            await Task.Delay(150);
            var resultAfter = await _cache.GetAsync<TestObject>(key);

            // Assert
            Assert.IsNotNull(resultBefore);
            Assert.IsNull(resultAfter);
        }

        [TestMethod]
        public async Task SetAsync_MultipleValues_AllRetrievable()
        {
            // Arrange
            var testData = new[]
            {
                new { Key = "key1", Value = new TestObject { Id = 1, Name = "First" } },
                new { Key = "key2", Value = new TestObject { Id = 2, Name = "Second" } },
                new { Key = "key3", Value = new TestObject { Id = 3, Name = "Third" } }
            };

            // Act
            foreach (var item in testData)
            {
                await _cache.SetAsync(item.Key, item.Value, TimeSpan.FromMinutes(10));
            }

            // Assert
            for (int i = 0; i < testData.Length; i++)
            {
                var result = await _cache.GetAsync<TestObject>(testData[i].Key);
                Assert.IsNotNull(result);
                Assert.AreEqual(testData[i].Value.Id, result.Id);
                Assert.AreEqual(testData[i].Value.Name, result.Name);
            }
        }

        private class TestObject
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }
}

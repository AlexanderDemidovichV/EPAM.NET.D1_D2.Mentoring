using System;
using CacheFibonacci;
using CacheFibonacci.Interfaces;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class RedisCacheServiceTest
    {
        private IFibonacciCacheAsync cacheService;
        private IFibonacciFactory fibonacciFactory;

        [SetUp]
        public void Initialize()
        {
            cacheService = new RedisCache(TimeSpan.FromMinutes(15));
            fibonacciFactory = new FibonacciFactory();
        }

        [TearDown]
        public void Cleanup()
        {
            cacheService = null;
        }

        [Test]
        public void AddItemToCache()
        {
            var value = fibonacciFactory.GetFibonacciNumber(6);
            var key = value.GetHashCode().ToString();

            cacheService.AddAsync(key, value).GetAwaiter().GetResult();
            var valueFromCache = cacheService.GetAsync<int>(key).GetAwaiter().GetResult();

            Assert.IsNotNull(valueFromCache);
        }

        [Test]
        public void LoopCall()
        {
            var value = fibonacciFactory.GetFibonacciNumber(6);
            var key = value.GetHashCode().ToString();

            cacheService.AddAsync(key, value).GetAwaiter().GetResult();

            for (int i = 0; i < 200; i++)
            {
                var valueFromCache = cacheService.GetAsync<int>(key).GetAwaiter().GetResult();

                Assert.IsNotNull(valueFromCache);
                Assert.AreEqual(value, valueFromCache);
            }
        }
    }
}

using CacheFibonacci;
using CacheFibonacci.Interfaces;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CacheServiceTest
    {
        private IFibonacciCache cacheService;
        private IFibonacciFactory fibonacciFactory;

        [SetUp]
        public void Initialize()
        {
            cacheService = new RuntimeCaching();
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


            cacheService.Set(key, value);
            var valueFromCache = cacheService.Get(key);

            Assert.IsNotNull(valueFromCache);
            Assert.AreEqual(value, valueFromCache);
        }

        [Test]
        public void UpdateItemInCache()
        {
            var value = fibonacciFactory.GetFibonacciNumber(12);
            var key = value.GetHashCode().ToString();

            cacheService.Set(key, value);
            value = fibonacciFactory.GetFibonacciNumber(13);
            if (cacheService.Get(key) != null)
            {
                cacheService.Delete(key);
            }
            key = value.GetHashCode().ToString();
            cacheService.Set(key, value);

            var userFromCache = cacheService.Get(key);

            Assert.IsNotNull(userFromCache);
            Assert.AreEqual(value, userFromCache);
        }
    }
}

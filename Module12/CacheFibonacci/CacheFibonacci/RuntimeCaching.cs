using System.Runtime.Caching;
using CacheFibonacci.Interfaces;

namespace CacheFibonacci
{
    public class RuntimeCaching: IFibonacciCache
    {
        private readonly ObjectCache _cache;

        const string cacheKey = "Fibonacci_CacheKey_";

        public RuntimeCaching()
        {
            _cache = MemoryCache.Default;
        }

        public int? Get(string key)
        {
            return _cache.Get(cacheKey + key) as int?;
        }

        public T Get<T>(string key)
        {
            var value = _cache.GetCacheItem(key)?.Value;
            return (T)value;
        }

        public void Set(string key, object value)
        {
            _cache.Set(cacheKey + key, value, ObjectCache.InfiniteAbsoluteExpiration);
        }

        public void Delete(string key)
        {
            _cache.Remove(key);
        }
    }
}

using System.Runtime.Caching;
using CacheFibonacci.Interfaces;

namespace CacheFibonacci
{
    public class RuntimeCaching: IFibonacciCache
    {
        const string cacheKey = "Fibonacci_CacheKey_";

        public int? Get(int key)
        {
            return MemoryCache.Default.Get(cacheKey + key) as int?;
        }

        public void Set(int key, int? value)
        {
            MemoryCache.Default.Set(cacheKey + key, value, ObjectCache.InfiniteAbsoluteExpiration);
        }
    }
}

using System.Threading.Tasks;
using CacheFibonacci.Interfaces;

namespace CacheFibonacci
{
    public class DataService<T>
    {
        private const string LastItemKey = "key";

        private readonly IFibonacciCacheAsync cacheService;


        public DataService(IFibonacciCacheAsync cacheService)
        {
            this.cacheService = cacheService;
        }

        public async Task<T> GetLastAsync()
        {
            T result = await cacheService.GetAsync<T>(LastItemKey);

            if (result == null)
            {
                //result = await 
            }

            await cacheService.AddAsync(LastItemKey, result);

            return result;
        }

        public async Task AddAsync(T item)
        {
            await cacheService.DeleteAsync(LastItemKey);
        }
    }
}

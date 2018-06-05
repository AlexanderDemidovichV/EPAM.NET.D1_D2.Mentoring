using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheFibonacci.Interfaces;

namespace CacheFibonacci
{
    public class DataService<T>//: IFibonacciCacheAsync<T>
    {
        private const string LastItemKey = "key";

        private readonly IFibonacciCacheAsync<T> cacheService;


        public DataService(IFibonacciCacheAsync<T> cacheService)
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

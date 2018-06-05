using System;
using System.Threading.Tasks;
using CacheFibonacci.Interfaces;
using Newtonsoft.Json;
using Polly;
using StackExchange.Redis;

namespace CacheFibonacci
{
    public class RedisCache: IFibonacciCache
    {
        private const int RetryCount = 3;

        private readonly IDatabase cache;
        private readonly TimeSpan expiration;

        private ConnectionMultiplexer redis;

        public string Name { get; }

        public RedisCache(TimeSpan expiration)
        {
            cache = CacheContent.Current;
            this.expiration = expiration;
            Name = "RedisCache";
        }

        public async Task AddAsync(string key, object value)
        {
            await Policy.Handle<RedisConnectionException>()
                .WaitAndRetryAsync(RetryCount, i => TimeSpan.FromSeconds(Math.Pow(2, i)))
                .ExecuteAsync(() => cache.StringSetAsync(key, JsonConvert.SerializeObject(value), expiration));
        }

        public async Task DeleteAsync(string key)
        {
            await Policy.Handle<RedisConnectionException>()
                .WaitAndRetryAsync(RetryCount, i => TimeSpan.FromSeconds(Math.Pow(2, i)))
                .ExecuteAsync(() => cache.KeyDeleteAsync(key));
        }

        public async Task<T> GetAsync<T>(string key)
        {
            T result = default(T);

            await Policy.Handle<RedisConnectionException>()
                .WaitAndRetryAsync(RetryCount, i => TimeSpan.FromSeconds(Math.Pow(2, i)))
                .ExecuteAsync(async () => {
                    var value = await cache.StringGetAsync(key);
                    if (!value.IsNullOrEmpty)
                        result = JsonConvert.DeserializeObject<T>(value);
                });

            return result;
        }

        public int? Get(int key)
        {
            throw new NotImplementedException();
        }

        public void Set(int key, int? value)
        {
            throw new NotImplementedException();
        }
    }
}

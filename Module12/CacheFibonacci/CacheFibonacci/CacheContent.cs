using System;
using StackExchange.Redis;

namespace CacheFibonacci
{
    public class CacheContent
    {
        private const string connectionString = @"RedisFibonacciCache.redis.cache.windows.net:6380,password=iV10GSb7mN1AnopByBtHhHz7MJbekDa+K/8piGzR2RY=,ssl=True,abortConnect=False";

        private static readonly Lazy<ConnectionMultiplexer> lazyConnection = 
            new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));

        private static IConnectionMultiplexer connection => lazyConnection.Value;

        public static IDatabase Current => connection.GetDatabase();
    }
}

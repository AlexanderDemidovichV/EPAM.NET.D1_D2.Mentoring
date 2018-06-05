using System;
using StackExchange.Redis;

namespace CacheFibonacci
{
    public class CacheContent
    {
        private const string connectionString = "";

        private static readonly Lazy<ConnectionMultiplexer> lazyConnection = 
            new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));

        private static IConnectionMultiplexer connection => lazyConnection.Value;

        public static IDatabase Current => connection.GetDatabase();
    }
}

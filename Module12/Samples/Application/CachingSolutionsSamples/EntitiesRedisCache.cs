using System.Collections.Generic;
using NorthwindLibrary;
using StackExchange.Redis;
using System.IO;
using System.Runtime.Serialization;

namespace CachingSolutionsSamples
{
	class EntitiesRedisCache : IEntitiesCache
	{
		private ConnectionMultiplexer redisConnection;
		string prefix = "Cache_Categories";
		DataContractSerializer serializer = new DataContractSerializer(
			typeof(IEnumerable<Category>));

		public EntitiesRedisCache(string hostName)
		{
			redisConnection = ConnectionMultiplexer.Connect(hostName);
		}

		public IEnumerable<T> Get<T>(string forUser)
		{
			var db = redisConnection.GetDatabase();
			byte[] s = db.StringGet(prefix + forUser);
			if (s == null)
				return null;

			return (IEnumerable<T>)serializer
				.ReadObject(new MemoryStream(s));

		}

		public void Set<T>(string forUser, IEnumerable<T> categories)
		{
			var db = redisConnection.GetDatabase();
			var key = prefix + forUser;

			if (categories == null) {
				db.StringSet(key, RedisValue.Null);
			} else {
				var stream = new MemoryStream();
				serializer.WriteObject(stream, categories);
				db.StringSet(key, stream.ToArray());
			}
		}
	}
}

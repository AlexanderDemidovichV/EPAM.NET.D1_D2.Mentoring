using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using System.Linq;
using System.Threading;

namespace CachingSolutionsSamples
{
	[TestClass]
	public class CacheTests
	{
		[TestMethod]
		public void MemoryCache()
		{
			var entityManager = new EntityManager(new EntitiesMemoryCache());

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(entityManager.GetEntities<Category>().Count());
                Thread.Sleep(100);
			}
		}

		[TestMethod]
		public void RedisCache()
		{
			var entityManager = new EntityManager(new EntitiesRedisCache("localhost"));

			for (var i = 0; i < 10; i++)
			{
			    Console.WriteLine(entityManager.GetEntities<Category>().Count());
			    Console.WriteLine(entityManager.GetEntities<Customer>().Count());
                Thread.Sleep(100);
			}
		}
	}
}

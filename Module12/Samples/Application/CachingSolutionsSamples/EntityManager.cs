using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CachingSolutionsSamples
{
    public class EntityManager
    {
        private IEntitiesCache cache;

        public EntityManager(IEntitiesCache cache)
        {
            this.cache = cache;
        }

        public IEnumerable<T> GetEntities<T>() where T : class
        {
            Console.WriteLine($"Getting {typeof(T)}.");

            var user = Thread.CurrentPrincipal.Identity.Name;
            var entities = cache.Get<T>(user);

            if (entities == null) {
                Console.WriteLine("From DB");

                using (var dbContext = new Northwind()) {
                    dbContext.Configuration.LazyLoadingEnabled = false;
                    dbContext.Configuration.ProxyCreationEnabled = false;
                    entities = dbContext.Set<T>().ToList();
                    cache.Set(user, entities);
                }
            }

            return entities;
        }
    }
}

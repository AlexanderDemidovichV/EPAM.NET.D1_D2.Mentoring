using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Caching;
using System.Data.SqlClient;

namespace CachingSolutionsSamples
{
	internal class EntitiesMemoryCache : IEntitiesCache
	{
		ObjectCache cache = MemoryCache.Default;
		string prefix  = "Cache_Categories";

	    public EntitiesMemoryCache()
	    {
	        SqlDependency.Start(ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString);
        }

        public IEnumerable<T> Get<T>(string forUser)
		{
			return (IEnumerable<T>) cache.Get(prefix + forUser);
		}

		public void Set<T>(string forUser, IEnumerable<T> categories)
		{
		    var policy = new CacheItemPolicy();
		    using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString)) {
		        using (var command = new SqlCommand("SELECT [CategoryID] FROM [dbo].[Categories]", connection)) {
		            var dependency = new SqlDependency(command);
		            connection.Open();
		            command.ExecuteNonQuery();
		            policy.ChangeMonitors.Add(new SqlChangeMonitor(dependency));
		        }
		    }

		    cache.Set(prefix + forUser, categories, policy);
		}
	}
}

using System.Collections.Generic;

namespace CachingSolutionsSamples
{
	public interface IEntitiesCache
	{
		IEnumerable<T> Get<T>(string forUser);
		void Set<T>(string forUser, IEnumerable<T> categories);
	}
}

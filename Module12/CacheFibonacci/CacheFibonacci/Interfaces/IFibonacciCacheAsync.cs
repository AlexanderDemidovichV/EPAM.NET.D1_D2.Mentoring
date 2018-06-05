using System.Threading.Tasks;

namespace CacheFibonacci.Interfaces
{
    public interface IFibonacciCacheAsync<T>
    {
        Task AddAsync(string key, object value);

        Task DeleteAsync(string key);

        Task<T> GetAsync<T>(string key);
    }
}

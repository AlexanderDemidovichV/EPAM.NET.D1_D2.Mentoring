namespace CacheFibonacci.Interfaces
{
    public interface IFibonacciCache
    {
        int? Get(string key);

        T Get<T>(string key);

        void Set(string key, object value);

        void Delete(string key);
    }
}

namespace CacheFibonacci.Interfaces
{
    public interface IFibonacciCache
    {
        int? Get(int key);

        void Set(int key, int? value);
    }
}

namespace Task2
{
    public interface IParser<out T>
    {
        T Parse(string value);
    }
}

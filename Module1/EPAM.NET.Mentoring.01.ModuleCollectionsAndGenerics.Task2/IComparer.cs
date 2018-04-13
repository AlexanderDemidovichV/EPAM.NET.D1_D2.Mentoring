namespace EPAM.NET.Mentoring
{ 
    public interface IComparer<in T>
    {
        bool Compare(T lhs, object rhs);
    }
}

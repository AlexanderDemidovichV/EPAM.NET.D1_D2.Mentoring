using System;

namespace EPAM.NET.Mentoring
{
    public delegate bool Comparison<T>(T lhs, object rhs);

    public class ComparisonAdaptor<T> : IComparer<T>
    {

        private readonly Comparison<T> _comparison;

        public ComparisonAdaptor(Comparison<T> comparison)
        {
            this._comparison = comparison ?? throw new ArgumentNullException();
        }

        public bool Compare(T lhs, object rhs) => _comparison(lhs, rhs);
    }
}

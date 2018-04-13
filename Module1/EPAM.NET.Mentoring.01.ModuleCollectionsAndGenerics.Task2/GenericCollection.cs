using System.Collections;
using System.Collections.Generic;

namespace EPAM.NET.Mentoring
{
    public class GenericCollection<T>: IEnumerable<T>
    {
        public IEnumerable<T> List { get; }

        public GenericCollection(IEnumerable<T> list)
        {
            this.List = list;
        }

        public IEnumerable<bool> Compare(GenericCollection<T> collection, object o, IComparer<T> comparer)
        {
            foreach (var value in collection) {
                yield return comparer.Compare(value, o);
            }
        }

        public IEnumerable<bool> Compare(GenericCollection<T> collection, object o, Comparison<T> comparison)
        {
            return Compare(collection, o, new ComparisonAdaptor<T>(comparison));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

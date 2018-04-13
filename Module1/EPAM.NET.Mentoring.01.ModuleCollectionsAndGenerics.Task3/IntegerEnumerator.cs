using System;
using System.Collections;
using System.Collections.Generic;

namespace EPAM.NET.Mentoring
{
    public class IntegerCollection: IEnumerable
    {
        private List<int> list;

        public IntegerCollection(IEnumerable<int> collection)
        {
            if (collection == null)
                throw new ArgumentNullException($"{nameof(collection)} is null.");

            list = new List<int>();

            using (IEnumerator<int> enumerator = collection.GetEnumerator()) {
                while (enumerator.MoveNext())
                    list.Add(enumerator.Current);
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new IntegerEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int GetElement(int index)
        {
            return list[index];
        }

        public int Count()
        {
            return list.Count;
        }

        private class IntegerEnumerator : IEnumerator<int>
        {
            private readonly IntegerCollection list;

            private int index;

            private int currentElement;

            public IntegerEnumerator(IntegerCollection list) {
                this.list = list;
                index = -1;
                currentElement = 0;
            }

            public int Current
            {
                get {
                    if (index < 0) {
                        if (index == -1) {
                            throw new InvalidOperationException("Enum not started.");
                        }
                        throw new InvalidOperationException("Enum ended.");
                    }
                    return currentElement;
                }
            }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (index == -2)
                    return false;
                index++;
                if (index == list.Count()) {
                    index = -2;
                    currentElement = 0;
                    return false;
                }
                currentElement = list.GetElement(index);
                return true;
            }

            public void Reset()
            {
                index = -1;
                currentElement = 0;
            }

            public void Dispose()
            {
                index = -2;
                currentElement = 0;
            }
        }
    }
    
}

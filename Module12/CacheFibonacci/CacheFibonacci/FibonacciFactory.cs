using System;
using CacheFibonacci.Interfaces;

namespace CacheFibonacci
{
    public class FibonacciFactory: IFibonacciFactory
    {
        public int GetFibonacciNumber(int index)
        {
            if (index < 0)
                throw new ArgumentException(nameof(index));

            if (index == 0 || index == 1)
                return 1;

            int previous = 0;
            int current = 1;
            int result = 1;

            for (int i = 0; i < index; i++) {
                result = current + previous;
                previous = current;
                current = result;
            }

            return result;
        }

    }
}

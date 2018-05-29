using System;
using System.Collections.Generic;
using CacheFibonacci.Interfaces;

namespace CacheFibonacci
{
    public class FibonacciFactory: IFibonacciFactory
    {
        public IEnumerable<long> GetFibonacciNumber()
        {
            yield return 1;
            yield return 1;

            long previous  = 1;
            long current = 1;

            while (true) {
                long next;

                try {
                    next = unchecked(previous + current);
                } catch (OverflowException) {
                    yield break;
                }

                yield return next;
                previous = current;
                current = next;
            }
        }

    }
}

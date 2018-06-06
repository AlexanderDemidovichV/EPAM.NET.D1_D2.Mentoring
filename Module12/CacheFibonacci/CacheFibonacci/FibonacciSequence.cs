using System;
using CacheFibonacci.Interfaces;

namespace CacheFibonacci
{
    public class FibonacciSequence
    {
        private IFibonacciFactory fibonacciFactory;
        private IFibonacciCache fibonacciCache;

        public FibonacciSequence(IFibonacciFactory fibonacciFactory, IFibonacciCache fibonacciCache)
        {
            this.fibonacciFactory = fibonacciFactory ?? throw new ArgumentNullException(nameof(fibonacciFactory));
            this.fibonacciCache = fibonacciCache ?? throw new ArgumentNullException(nameof(fibonacciCache));
        }

        public int? GetFibonacciNumber(int index)
        {
            if (index < 1) 
                throw new ArgumentException(nameof(index));

            var number = fibonacciCache.Get(index.ToString());
            if (!number.HasValue) {
                number = fibonacciFactory.GetFibonacciNumber(index);
                fibonacciCache.Set(index.ToString(), number);
            }

            return number;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheFibonacci.Interfaces;

namespace CacheFibonacci
{
    public class FibonacciSequence
    {
        private IFibonacciFactory fibonacciFactory;
        private IFibonacciCache fibonacciCache;

        public FibonacciSequence(IFibonacciFactory fibonacciFactory, IFibonacciCache fibonacciCache)
        {
            if (fibonacciFactory == null)
                throw new ArgumentNullException(nameof(fibonacciFactory));

            if (fibonacciCache == null)
                throw new ArgumentNullException(nameof(fibonacciCache));

            this.fibonacciFactory = fibonacciFactory;
            this.fibonacciCache = fibonacciCache;
        }

        public IEnumerable<long> GetFibonacciNumber()
        {
            foreach (var number in fibonacciFactory.GetFibonacciNumber())
            {
                if (checkCache())
                    setCache();
                yield return number;
            }
            

        }

        private bool checkCache()
        {
            return false;
        }

        private void setCache()
        {
            
        }
    }
}

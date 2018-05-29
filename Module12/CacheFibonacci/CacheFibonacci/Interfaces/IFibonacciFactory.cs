using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheFibonacci.Interfaces
{
    public interface IFibonacciFactory
    {
        IEnumerable<long> GetFibonacciNumber();
    }
}

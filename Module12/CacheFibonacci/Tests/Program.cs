using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheFibonacci;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var y = new FibonacciFactory();
            while (true)
            {
                //foreach (var VARIABLE in FibonacciFactory.GetFibonacciNumber())
                //{
                //    long t = VARIABLE;
                //}

                for (int i = 0; i < 1000; i++)
                {
                    var t = y.GetFibonacciNumber(i);
                }
            }
        }
    }
}

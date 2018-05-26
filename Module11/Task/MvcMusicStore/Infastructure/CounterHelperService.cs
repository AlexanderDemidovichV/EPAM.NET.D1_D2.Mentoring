using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PerformanceCounterHelper;

namespace MvcMusicStore.Infastructure
{
    public class CounterHelperService
    {
        private static readonly Lazy<CounterHelperService> lazy =
            new Lazy<CounterHelperService>(() => new CounterHelperService());

        private CounterHelper<Counters> CounterHelper;
    
        public static CounterHelperService Instance => lazy.Value;

        private CounterHelperService()
        {
            CounterHelper = PerformanceHelper.CreateCounterHelper<Counters>("CounterHelper");
        }

        public void Increment(Counters counter)
        {
            CounterHelper.Increment(counter);
        }
    }
}
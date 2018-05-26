using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PerformanceCounterHelper;

namespace MvcMusicStore.Infastructure
{
    [PerformanceCounterCategory("MvcMusicStore", System.Diagnostics.PerformanceCounterCategoryType.MultiInstance, "MvcMusicStore")]
    public enum Counters
    {
        [PerformanceCounter("Go Home counter", "Go Home", System.Diagnostics.PerformanceCounterType.NumberOfItems32)]
        GoToHome,
        [PerformanceCounter("Successful LogOut counter", "Go LogOut", System.Diagnostics.PerformanceCounterType.NumberOfItems32)]
        SuccessfulLogOut,
        [PerformanceCounter("Successful LogIn counter", "Go LogIn", System.Diagnostics.PerformanceCounterType.NumberOfItems32)]
        SuccessfulLogIn,
    }
}
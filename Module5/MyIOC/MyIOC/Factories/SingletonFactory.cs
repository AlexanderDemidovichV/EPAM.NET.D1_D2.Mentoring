using System;

namespace MyIOC.Factories
{
    public class SingletonFactory : IFactoryProvider
    {
        private Lazy<object> singletonCreator;

        public SingletonFactory(Delegate objectCreator)
        {
            singletonCreator = new Lazy<object>(() => objectCreator.DynamicInvoke());
        }

        public object Create()
        {
            return singletonCreator.Value;
        }
    }
}

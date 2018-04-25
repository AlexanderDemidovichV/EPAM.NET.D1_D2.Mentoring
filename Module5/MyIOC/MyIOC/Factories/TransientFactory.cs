using System;

namespace MyIOC.Factories
{
    public class TransientFactory : IFactoryProvider
    {
        private Delegate objectCreator;

        public TransientFactory(Delegate objectCreator)
        {
            this.objectCreator = objectCreator;
        }

        public object Create()
        {
            return objectCreator.DynamicInvoke();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyIOC.Attributes;

namespace MyIOC.Test
{
    public class SomeIBazDependantClass
    {
        [ImportConstructor]
        public SomeIBazDependantClass(IBaz baz)
        {
            ConstructorSatisfiedIBaz = baz;
        }

        public IBaz ConstructorSatisfiedIBaz { get; private set; }

        [Import]
        public IBaz SomePropBaz { get; set; }
    }
}

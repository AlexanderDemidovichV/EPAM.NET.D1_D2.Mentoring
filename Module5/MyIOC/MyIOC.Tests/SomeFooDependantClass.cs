using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyIOC.Attributes;

namespace MyIOC.Test
{
    public class SomeFooDependantClass
    {
        [ImportConstructor]
        public SomeFooDependantClass(Foo foo, int age, IBaz baz)
        {
            ConstructorSatisfiedFoo = foo;
            ConstructorSatisfiedAge = age;
            ConstructorSatisfiedIBaz = baz;
        }

        public Foo ConstructorSatisfiedFoo { get; private set; }
        public int ConstructorSatisfiedAge { get; private set; }
        public IBaz ConstructorSatisfiedIBaz { get; private set; }

    }
}

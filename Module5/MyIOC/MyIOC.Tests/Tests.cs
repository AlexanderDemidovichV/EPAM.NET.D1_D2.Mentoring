using System;
using System.Reflection;
using MyIOC.Enums;
using MyIOC.Registration;
using NUnit.Framework;

namespace MyIOC.Test
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Resolve()
        {
            Container container = new Container();

            container.AddType(typeof(Foo));
            container.AddType(typeof(IBaz), typeof(Baz));

            container.RegisterComponents(
                new Component().For(typeof(SomeFooDependantClass))
                    .DependsOn(new {
                        age = 1
                    })
                    .WithInstanceMode(InstanceMode.Transient)
            );

            Assert.DoesNotThrow(() =>
            {
                container.WireUp();
            });

            SomeFooDependantClass y1 = container.CreateInstance<SomeFooDependantClass>();
            SomeFooDependantClass y2 = container.CreateInstance<SomeFooDependantClass>();

            Assert.IsFalse(ReferenceEquals(y1, y2));
        }

        [Test]
        public void ResolveWithExportAssembly()
        {
            Container container = new Container();

            container.AddAssembly(Assembly.GetExecutingAssembly());

            container.RegisterComponents(
                new Component().For(typeof(SomeFooDependantClass))
                    .DependsOn(new {
                        age = 1
                    })
                    .WithInstanceMode(InstanceMode.Transient)
            );

            Assert.DoesNotThrow(() => {
                container.WireUp();
            });

            SomeFooDependantClass y1 = container.CreateInstance<SomeFooDependantClass>();
            SomeFooDependantClass y2 = container.CreateInstance<SomeFooDependantClass>();

            Assert.IsFalse(ReferenceEquals(y1, y2));
        }

        [Test]
        public void TestClassWithProperty()
        {
            Container container = new Container();
            int someMockAge = 23;

            container.AddAssembly(Assembly.GetExecutingAssembly());

            container.AddType(typeof(SomeIBazDependantClass), InstanceMode.Singleton);
            container.RegisterComponents(
                new Component().For(typeof(SomeFooDependantClass))
                    .DependsOn(new {
                        age = someMockAge
                    })
                    .WithInstanceMode(InstanceMode.Transient)
            );

            Assert.DoesNotThrow(() => {
                container.WireUp();
            });

            SomeIBazDependantClass x1 = container.CreateInstance<SomeIBazDependantClass>();
            SomeIBazDependantClass x2 = container.CreateInstance<SomeIBazDependantClass>();
            bool ibazDependantAreSame = Object.ReferenceEquals(x1, x2);
            Assert.IsTrue(ibazDependantAreSame);

            SomeFooDependantClass y1 = container.CreateInstance<SomeFooDependantClass>();
            SomeFooDependantClass y2 = container.CreateInstance<SomeFooDependantClass>();
            bool fooDependantAreSame = Object.ReferenceEquals(y1, y2);
            Assert.IsFalse(fooDependantAreSame);
        }
    }
}

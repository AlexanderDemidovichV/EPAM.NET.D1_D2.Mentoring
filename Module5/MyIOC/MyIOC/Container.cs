using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using MyIOC.Attributes;
using MyIOC.Enums;
using MyIOC.Exceptions;
using MyIOC.Factories;
using MyIOC.Registration;

namespace MyIOC
{
    public class Container : IContainer
    {
        private readonly object syncLock = new object();
        private readonly Dictionary<ComponentRegistration, IFactoryProvider> components = 
            new Dictionary<ComponentRegistration, IFactoryProvider>();


        public void AddType(Type type)
        {
            AddType(type, InstanceMode.Transient);
        }

        public void AddType(Type type, InstanceMode mode)
        {
            components.Add(new Component().For(type).WithInstanceMode(mode), null);
        }

        public void AddType(Type typeFrom, Type typeTo)
        {
            AddType(typeFrom, typeTo, InstanceMode.Transient);
        }

        public void AddType(Type typeFrom, Type typeTo, InstanceMode mode)
        {
            components.Add(new Component().ServiceFor(typeFrom, typeTo).
                WithInstanceMode(mode), null);
        }

        public void AddAssembly(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes()) {
                if (type.GetCustomAttributes(typeof(ExportAttribute), true).Length > 0) {
                    var attr = (ExportAttribute)Attribute.
                        GetCustomAttribute(type, typeof(ExportAttribute));
                    var contract = attr.Contract;
                    if (contract == null)
                        AddType(type);
                    else
                        AddType(contract, type);
                }
            }
        }

        public void RegisterComponents(params ComponentRegistration[] registrations)
        {
            lock (syncLock) {
                foreach (ComponentRegistration componentRegistration in registrations.ToList()) {
                    components.Add(componentRegistration, null);
                }
            }
        }

        public void WireUp()
        {
            foreach (ComponentRegistration key in components.
                Where(c => c.Value == null).Select(c => c.Key).ToList()) {
                CreateFactory(key, GetConstructorDelegateForType(key.TypeToCreate),
                    key.InstanceMode);
            }
        }

        public T CreateInstance<T>()
        {
            lock (syncLock) {
                IFactoryProvider creator = components.
                    Where(x => x.Key.TypeToCreate == typeof(T)).
                    Select(x => x.Value).SingleOrDefault();

                if (creator != null) {
                    T newlyCreatedObject = (T)creator.Create();
                    SatisfyProperties<T>(newlyCreatedObject);
                    return newlyCreatedObject;
                }
                throw new ContainerConfigurationException(string.Format(
                    "Couldn't create instance of {0} could not find correct " +
                    "IFactoryProvider. This may be down to missing Component registration", 
                    typeof(T).FullName));
            }
        }


        private void SatisfyProperties<T>(T newlyCreatedObject)
        {
            foreach (PropertyInfo prop in newlyCreatedObject.GetType().GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(ImportAttribute), false).Any()))
            {
                IFactoryProvider factoryProvider = components.
                    Single(x => x.Key.TypeToCreate == prop.PropertyType || 
                    prop.PropertyType.IsAssignableFrom(x.Key.TypeToLookFor)).Value;

                if (factoryProvider != null) {
                    prop.SetValue(newlyCreatedObject, factoryProvider.Create(), null);
                } else {
                    throw new ContainerConfigurationException(string.Format(
                        "Couldn't find instance of {0} to use for property injection", 
                        prop.PropertyType.FullName));
                }
            }
        }

        private Delegate GetConstructorDelegateForType(Type type)
        {
            ConstructorInfo ctor = type.GetConstructors().
                SingleOrDefault(x => x.GetCustomAttributes(typeof(ImportConstructorAttribute), false).Any()) 
                ?? type.GetConstructors()[0];

            List<ConstructorParameterDependency> args = new List<ConstructorParameterDependency>();

            foreach (var ctorArg in ctor.GetParameters()) {
                CheckArgument(ctorArg, type, args);
            }

            var componentRegistration = FetchComponentRegistration(type);
            if (componentRegistration != null) {
                if (componentRegistration.DependsOnValues.Any()) {
                    args.AddRange(componentRegistration.DependsOnValues);
                }
            }

            return Expression.Lambda(Expression.New(ctor, args.OrderBy(x => x.Position)
                .Select(x => x.Value).ToArray())).Compile();
        }

        private void CheckArgument(ParameterInfo ctorArg, Type type, 
            List<ConstructorParameterDependency> args)
        {
            bool isParamCoveredByManualRegistration = IsParamCoveredByManualRegistration(type, ctorArg);
            if (!isParamCoveredByManualRegistration) {
                bool parameterKeyFound = components.Keys.Any(x => x.TypeToCreate == ctorArg.ParameterType ||
                                                             ctorArg.ParameterType.IsAssignableFrom(x.TypeToLookFor));
                if (!parameterKeyFound) {
                    throw new ContainerConfigurationException(string.Format("Couldn't find ctor argument {0}", ctorArg.GetType()));
                }
                var componentRegistration = FetchComponentRegistration(ctorArg.ParameterType);
                if (components[componentRegistration] == null) {
                    Delegate delegateForType = GetConstructorDelegateForType(componentRegistration.TypeToCreate);
                    CreateFactory(componentRegistration, delegateForType, componentRegistration.InstanceMode);
                }

                args.Add(new ConstructorParameterDependency(
                    ctorArg.ParameterType,
                    ctorArg.Name,
                    Expression.Constant(components[componentRegistration].Create()),
                    ctorArg.Position));
            }
        }


        private ComponentRegistration FetchComponentRegistration(Type typeToLookFor)
        {
            ComponentRegistration componentRegistration = 
                components.Single(x => x.Key.TypeToCreate == typeToLookFor || 
                    typeToLookFor.IsAssignableFrom(x.Key.TypeToLookFor)).Key;
            return componentRegistration;
        }


        private bool IsParamCoveredByManualRegistration(Type constructorOwnerType, ParameterInfo constructorArg)
        {
            ComponentRegistration componentRegistration = FetchComponentRegistration(constructorOwnerType);
            if (!componentRegistration.HasManualConstructorParameters) {
                return false;
            }
            var constructorParameterDependency =
                componentRegistration.DependsOnValues.SingleOrDefault(
                    x => x.ArgType == constructorArg.ParameterType && x.Name == constructorArg.Name);

            if (constructorParameterDependency == null)
                return false;

            constructorParameterDependency.Position = constructorArg.Position;
            return true;
        }

        private void CreateFactory(ComponentRegistration key, Delegate factoryDelegate, InstanceMode instanceMode)
        {
            IFactoryProvider factoryProvider = null;
            
            if (instanceMode == InstanceMode.Transient) {
                factoryProvider = new TransientFactory(factoryDelegate);
            }

            if (instanceMode == InstanceMode.Singleton) {
                factoryProvider = new SingletonFactory(factoryDelegate);
            }

            lock (syncLock) {
                components[key] = factoryProvider;
            }
        }
    }
}

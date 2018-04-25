using System;
using System.Linq.Expressions;

namespace MyIOC.Registration
{
    public class ConstructorParameterDependency
    {
        public Type ArgType { get; private set; }
        public string Name { get; private set; }
        public ConstantExpression Value { get; private set; }
        public int Position { get; set; }

        public ConstructorParameterDependency(Type argType, string name, ConstantExpression value)
        {
            ArgType = argType;
            Name = name;
            Value = value;
        }

        public ConstructorParameterDependency(Type argType, string name, ConstantExpression value, int position)
        {
            ArgType = argType;
            Name = name;
            Value = value;
            Position = position;
        }
    }
}

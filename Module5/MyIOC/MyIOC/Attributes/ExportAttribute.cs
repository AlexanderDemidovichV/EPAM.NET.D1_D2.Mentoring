using System;
using MyIOC.Enums;

namespace MyIOC.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExportAttribute : Attribute
    {
        public ExportAttribute()
        { }

        public ExportAttribute(InstanceMode mode)
        {
            Mode = mode;
        }

        public ExportAttribute(Type contract)
        {
            Contract = contract;
        }

        public Type Contract { get; private set; }
        public InstanceMode Mode { get; private set; }
    }
}

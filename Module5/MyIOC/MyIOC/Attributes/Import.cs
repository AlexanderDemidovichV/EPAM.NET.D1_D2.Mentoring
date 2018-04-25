using System;

namespace MyIOC.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class ImportAttribute : Attribute
    {
    }
}

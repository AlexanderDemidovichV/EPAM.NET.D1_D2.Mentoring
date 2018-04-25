using MyIOC.Attributes;

namespace MyIOC.Test
{
    [Export(typeof(IBaz))]
    public class Baz : IBaz
    {
    }
}
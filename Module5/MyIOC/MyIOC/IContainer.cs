using MyIOC.Registration;

namespace MyIOC
{
    public interface IContainer
    {
        void RegisterComponents(params ComponentRegistration[] registrations);
        void WireUp();
        T CreateInstance<T>();
    }
}

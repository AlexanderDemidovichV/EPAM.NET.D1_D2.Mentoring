using Autofac;
using Autofac.Integration.Mvc;
using LogProvider;
using MvcMusicStore.Controllers;

namespace MvcMusicStore.App_Start
{
    public static class AutofacTypeRegistration
    {
        public static ContainerBuilder CreateContainerBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(HomeController).Assembly);
            builder.Register(f => Log4NetProvider.LogProvider("ForControllers")).As<LogAdapter.ILogger>();

            return builder;
        }
    }
}
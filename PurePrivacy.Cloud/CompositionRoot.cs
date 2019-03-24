using Autofac;
using Microsoft.Extensions.Logging;
using PurePrivacy.Client;
using PurePrivacy.Cloud.Applications;
using PurePrivacy.Server;

namespace PurePrivacy.Cloud
{
    internal class CompositionRoot
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();

            RegisterLogger(builder);

            builder.RegisterType<CloudApplication>().As<IApplication>();
            //builder.RegisterType<ClientApplication>().As<IApplication>();

            builder.RegisterModule(new ClientModule());
            builder.RegisterModule(new ServerModule());

            return ConfigureLogger(builder.Build());
        }

        private IContainer ConfigureLogger(IContainer container)
        {
            // https://github.com/aspnet/Extensions/issues/615#issuecomment-447061661
            var loggerFactory = container.Resolve<ILoggerFactory>();
            loggerFactory.AddConsole();

            return container;
        }

        private void RegisterLogger(ContainerBuilder builder)
        {
            builder.RegisterType<LoggerFactory>().As<ILoggerFactory>().SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();
            builder.Register(context => context.Resolve<ILoggerFactory>().CreateLogger("Default")).As<ILogger>();
        }
    }
}
using Autofac;
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

            builder.RegisterType<CloudApplication>().As<IApplication>();

            builder.RegisterModule(new ClientModule());
            builder.RegisterModule(new ServerModule());

            return builder.Build();
        }
    }
}
using Autofac;

namespace PurePrivacy.Client
{
    public class ClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BasicClient>().As<IClient>();
        }
    }
}

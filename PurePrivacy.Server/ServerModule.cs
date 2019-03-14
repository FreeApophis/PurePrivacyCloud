using Autofac;

namespace PurePrivacy.Server
{

    public class ServerModule : Module
    {
        private int _currentPort = 1982;
        private int GetNextPort()
        {
            return _currentPort++;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context => new BlockServer(GetNextPort())).As<IBlockServer>();
        }
    }
}

using Autofac;
using Autofac.Core;

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
            builder.RegisterType<BlockServer>().As<IBlockServer>().WithParameter(
                new ResolvedParameter(
                    (parameter, context) => parameter.Name == "port",
                    (parameter, context) => GetNextPort())
                );
        }
    }
}

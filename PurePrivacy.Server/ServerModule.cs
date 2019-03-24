using System;
using System.Reflection;
using Autofac;
using Autofac.Core;
using PurePrivacy.Protocol;
using PurePrivacy.Server.MessageHandler;
using Module = Autofac.Module;

namespace PurePrivacy.Server
{

    public class ServerModule : Module
    {
        private const string TypeField = "Type";
        private const string PortParameter = "port";

        private int _currentPort = 1982;

        private int GetNextPort()
        {
            return _currentPort++;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BlockServer>().As<IBlockServer>().WithParameter(
                new ResolvedParameter(
                    (parameter, context) => parameter.Name == PortParameter,
                    (parameter, context) => GetNextPort())
            );

            builder.RegisterType<DictionaryBlockStorage>().As<IBlockStorage>().SingleInstance();

            // Register all Handlers
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => typeof(MessageHandler.MessageHandler).IsAssignableFrom(t))
                .Keyed<MessageHandler.MessageHandler>(type => HandlerKey(type));
        }

        private static MessageType HandlerKey(Type type)
        {
            var field = type.GetField(TypeField, BindingFlags.NonPublic | BindingFlags.Static);

            return field == null ? MessageType.Invalid : (MessageType)field.GetRawConstantValue();
        }
    }
}

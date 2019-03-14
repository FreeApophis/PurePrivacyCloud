using Autofac;
using PurePrivacy.Cloud;

namespace PurePrivacyCloud
{
    class Program
    {
        private static void Main(string[] args)
        {
            new CompositionRoot()
                .Build()
                .Resolve<IApplication>()
                .Run(args);
        }
    }
}

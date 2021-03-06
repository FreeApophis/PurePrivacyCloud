﻿using Autofac;
using PurePrivacy.Cloud.Applications;

namespace PurePrivacy.Cloud
{
    class Program
    {
        private static void Main(string[] args)
        {
            new CompositionRoot()
                .Build()
                .BeginLifetimeScope()
                .Resolve<IApplication>()
                .Run(args);
        }

        private static void DoWork()
        {
        }
    }
}

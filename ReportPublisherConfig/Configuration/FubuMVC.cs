﻿using Bottles;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using ReportPublisherConfig.Configuration.Registries;
using StructureMap;

namespace ReportPublisherConfig.Configuration
{
    public static class FubuMVC
    {
        public static void Start()
        {
            // FubuApplication "guides" the bootstrapping of the FubuMVC
            // application
            FubuApplication.For<ConfigureFubuMVC>() // ConfigureFubuMVC is the main FubuRegistry
                // for this application.  FubuRegistry classes 
                // are used to register conventions, policies,
                // and various other parts of a FubuMVC application


                // FubuMVC requires an IoC container for its own internals.
                // In this case, we're using a brand new StructureMap container,
                // but FubuMVC just adds configuration to an IoC container so
                // that you can use the native registration API's for your
                // IoC container for the rest of your application
                .StructureMap(ObjectFactory.Container)
                .StructureMapObjectFactory(configure => configure.AddRegistry<DefaultConventionsRegistry>())
                .Bootstrap();

            // Ensure that no errors occurred during bootstrapping
            PackageRegistry.AssertNoFailures();
        }
    }
}
using ReportPublisherConfig.Configuration.ApplicationStartupTasks;
using StructureMap.Configuration.DSL;

namespace ReportPublisherConfig.Configuration.Registries
{
    public class DefaultConventionsRegistry : Registry
    {
        public DefaultConventionsRegistry()
        {
            Scan(scanning =>
            {
                scanning.TheCallingAssembly();
                scanning.AddAllTypesOf<IApplicationStartupTask>();
                scanning.WithDefaultConventions();
            });
        }
    }
}
using System.Linq;
using ReportPublisherConfig.Configuration.ApplicationStartupTasks;
using StructureMap;

namespace ReportPublisherConfig.Configuration
{
    public class Bootstrapper
    {
        public static void Bootstrap()
        {
            FubuMVC.Start();
            foreach (var applicationStartupTask in ObjectFactory.GetAllInstances<IApplicationStartupTask>().OrderBy(x => x.GetType().Name))
            {
                applicationStartupTask.Execute();
            }
        }
    }
}
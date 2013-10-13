using FubuMVC.Core;
using FubuMVC.Razor;
using ReportPublisherConfig.Configuration.Conventions;
using ReportPublisherConfig.Handlers;

namespace ReportPublisherConfig.Configuration.Registries
{
    public class ConfigureFubuMVC : FubuRegistry
    {
        public ConfigureFubuMVC()
        {
            Assets.YSOD_on_missing_assets(true);
            
            // This line turns on the basic diagnostics and request tracing
            IncludeDiagnostics(true);
            ApplyHandlerConventions<HandlersMarker>();

            // Where is our home page?
            Routes.HomeIs<Handlers.Home.GetHandler>(x => x.Execute());
            
            //Use Razor!
            Import<RazorEngineRegistry>();

            // Match views to action methods by matching
            // on model type, view name, and namespace
            Views.TryToAttachWithDefaultConventions();
            
            ApplyConvention<AjaxConvention>();
        }
    }
}
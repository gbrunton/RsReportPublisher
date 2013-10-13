using Newtonsoft.Json;

namespace ReportPublisherConfig
{
    public static class ObjectExtensionMethods
    {
        public static string ToJSON(this object entity)
        {
            return JsonConvert.SerializeObject(entity);
        } 
    }
}
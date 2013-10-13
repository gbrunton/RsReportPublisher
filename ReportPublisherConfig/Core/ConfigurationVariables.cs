using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ReportPublisherConfig.Models;

namespace ReportPublisherConfig.Core
{
    public class ConfigurationVariables
    {
        public string GetPathToConfigFile()
        {
			var appSettings = System.Web.Configuration.WebConfigurationManager.AppSettings;
			 var index = appSettings
				.AllKeys
				.Single(x => x == "PathToConfig");
			return appSettings[index];
        }

        public string GetReportPublisherProgramPath()
        {
            return this.GetReportPublisherBinFolderPath() + "TrustFundAEA.ReportPublisher";
        }

        public string GetReportPublisherBinFolderPath()
        {
            return getReportPublisherRoot() + @"bin\";
        }

        public IEnumerable<Site> GetSites()
        {
            var appSettings = System.Web.Configuration.WebConfigurationManager.AppSettings;
            return appSettings
                .AllKeys.Where(x => x.StartsWith("site-"))
                .Select(x => new Site
                {
                    Name = x.Replace("site-", string.Empty),
                    Url = appSettings[x]
                });
        }

        private static string getReportPublisherRoot()
        {
            return Path.GetFullPath(HttpContext.Current.Request.PhysicalApplicationPath + @"\..\..\TrustFundAEA.ReportPublisher\");
        }
    }
}
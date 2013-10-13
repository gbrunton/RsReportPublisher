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
	        return this.getFromAppSettings("PathToConfig");
        }

		public string GetReportServerRootFolder()
		{
			return this.getFromAppSettings("ReportServerRootFolder");
		}

		private string getFromAppSettings(string keyName)
		{
			var appSettings = System.Web.Configuration.WebConfigurationManager.AppSettings;
			var index = appSettings
			   .AllKeys
			   .Single(x => x == keyName);
			return appSettings[index];
		}

		public string GetReportPublisherProgramPath()
        {
            return this.GetReportPublisherBinFolderPath() + "Publisher.exe";
        }

        public string GetReportPublisherBinFolderPath()
        {
			return Path.GetFullPath(HttpContext.Current.Request.PhysicalApplicationPath + @"\..\tools\Publisher\");
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
    }
}
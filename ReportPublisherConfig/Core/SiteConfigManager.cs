using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace ReportPublisherConfig.Core
{
    public class SiteConfigManager : IDisposable
    {
        private readonly ConfigurationVariables configurationVariables;

        public SiteConfigManager(ConfigurationVariables configurationVariables)
        {
            if (configurationVariables == null) throw new ArgumentNullException("configurationVariables");
            this.configurationVariables = configurationVariables;
        }

        public void Update(string site)
        {
            var pathToSiteConfigFile = this.configurationVariables.GetPathToSiteConfigFile();
            File.Copy(pathToSiteConfigFile, getCopyPath(pathToSiteConfigFile), true);

            var document = new XmlDocument();
            document.Load(pathToSiteConfigFile);
            foreach (XPathNavigator nav in document.CreateNavigator().Select("//TrustFundAEA.ReportPublisher.Properties.Settings//setting//value"))
            {
                nav.SetValue(site);
            }
            document.Save(pathToSiteConfigFile);

            // The following works too but I don't like it as much.
            //var xdoc = XDocument.Load(pathToSiteConfigFile);
            //xdoc.Elements().First().Elements()
            //    .Where(x => x.Name == "applicationSettings")
            //    .Elements().First()
            //    .Elements().First()
            //    .Elements().First()
            //    .Value = "testing";
            //xdoc.Save(pathToSiteConfigFile + ".log");
        }

        public void Dispose()
        {
            var pathToSiteConfigFile = this.configurationVariables.GetPathToSiteConfigFile();
            var copyPath = getCopyPath(pathToSiteConfigFile);
            if (! File.Exists(copyPath)) return;
            File.Copy(copyPath, pathToSiteConfigFile, true);
            File.Delete(copyPath);
        }

        private static string getCopyPath(string pathToSiteConfigFile)
        {
            return pathToSiteConfigFile + ".copy";
        }
    }
}
using System;
using System.Diagnostics;
using FubuCore;
using ReportPublisherConfig.Configuration.Conventions;
using ReportPublisherConfig.Core;

namespace ReportPublisherConfig.Handlers.Home.publishReports
{
    public class PostHandler
    {
        private readonly ConfigurationVariables configurationVariables;
        private readonly SiteConfigManager siteConfigManager;

        public PostHandler(ConfigurationVariables configurationVariables, SiteConfigManager siteConfigManager)
        {
            if (configurationVariables == null) throw new ArgumentNullException("configurationVariables");
            if (siteConfigManager == null) throw new ArgumentNullException("siteConfigManager");
            this.configurationVariables = configurationVariables;
            this.siteConfigManager = siteConfigManager;
        }

        [AjaxAction]
        public string Execute(PublishReportsInputModel publishReportsInputModel)
        {
            this.siteConfigManager.Update(publishReportsInputModel.site);

            using (var process = new Process())
            {
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.WorkingDirectory = this.configurationVariables.GetReportPublisherBinFolderPath();
                process.StartInfo.Arguments = "/K {0} TrustFundAEA {1}"
                    .ToFormat
                    (
                        this.configurationVariables.GetReportPublisherProgramPath(),
                        publishReportsInputModel.folderName.ToUpper() == "CONFIGURATION" ? "all" : publishReportsInputModel.folderName
                     );
                process.Start();
                process.WaitForExit();
            }

            return string.Empty;
        }
    }

    public class PublishReportsInputModel
    {
        public string folderName { get; set; }
        public string site { get; set; }
    }
}
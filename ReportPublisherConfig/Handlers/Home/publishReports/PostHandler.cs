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

        public PostHandler(ConfigurationVariables configurationVariables)
        {
            if (configurationVariables == null) throw new ArgumentNullException("configurationVariables");
            this.configurationVariables = configurationVariables;
        }

        [AjaxAction]
        public string Execute(PublishReportsInputModel publishReportsInputModel)
        {
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
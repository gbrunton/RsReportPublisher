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
	            process.StartInfo.Arguments = "/K {0} {1} {2} {3} {4} -p {5}"
		            .ToFormat
		            (
			            this.configurationVariables.GetReportPublisherProgramPath(),
						"publish",
						publishReportsInputModel.site,
						this.configurationVariables.GetPathToConfigFile(),
						this.configurationVariables.GetReportServerRootFolder(),
			            publishReportsInputModel.folderName.ToUpper() == "CONFIGURATION"
				            ? ""
				            : publishReportsInputModel.folderName
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
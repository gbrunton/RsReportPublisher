using System;
using System.Collections.Generic;
using FubuCore.CommandLine;
using Publisher.Commands.PublishReports.Models;
using Publisher.Commands.PublishReports.Models.Config;
using Publisher.Commands.PublishReports.Models.Web;
using Publisher.WebReportService;

namespace Publisher.Commands.PublishReports
{
	[CommandDescription("Publish reports.", Name = "publish")]
	public class Command : Command<InputModel>
	{
		protected override bool ExecuteHydratedObject(InputModel inputModel)
		{
			StaticVariables.PathToConfigurationFile = inputModel.PathToConfigurationFile;

			Properties.Settings.Default.Publisher_WebReportService_ReportingService2005 = inputModel.ReportServerUrl;
			Properties.Settings.Default.Save();
			var reportServerRootFolder = inputModel.ReportServerRootFolder;
			var projectName = inputModel.ProjectName;
			var configConfiguration = new ConfigConfiguration();

			// Setting up root level policies
			var rootFolder = new WebFolder(null, "") { DeleteExistingFolders = false };
			addNewPolicies(rootFolder, configConfiguration.ConfigPolicies());
			rootFolder.Save();

			foreach (var configFolder in configConfiguration.Folders())
			{
				if (!configFolder.Name.Equals(projectName)) continue;

				Console.WriteLine("Starting Project {0} report publishing.", configFolder.Name);

				var webFund = new WebFund(reportServerRootFolder, configFolder.Name, configFolder.DataSource.Name, configFolder.InheritPermissions);

				build(configFolder, webFund.RootWebFolder);
				webFund.Save();

				Console.WriteLine("Completed Project {0} report publishing.", configFolder.Name);
			}
			return true;
		}

		private static void build(ConfigFolder configFolder, WebFolder webFolder)
		{
			foreach (var tempConfigFolder in configFolder.Folders())
			{
				var newWebFolder = webFolder.AddWebFolder(tempConfigFolder.Name, tempConfigFolder.InheritPermissions);

				addNewPolicies(newWebFolder, tempConfigFolder.ConfigPolicies());
				addNewProperty(newWebFolder, tempConfigFolder.ConfigProperties());

				foreach (var tempConfigReport in tempConfigFolder.Reports())
				{
					var newWebReport = newWebFolder.AddWebReport(tempConfigReport.Name, tempConfigReport.Definition(), tempConfigReport.InheritPermissions);

					addNewPolicies(newWebReport, tempConfigReport.ConfigPolicies());
					addNewProperty(newWebReport, tempConfigReport.ConfigProperties());

					foreach (var configReportParameter in tempConfigReport.ConfigReportParameters())
					{
						newWebReport.AddNewReportParameter(configReportParameter.Name, configReportParameter.Value);
					}
				}

				foreach (var tempConfigStyleSheet in tempConfigFolder.StyleSheets())
				{
					var newWebStyleSheet = newWebFolder.AddWebStyleSheet(tempConfigStyleSheet.Name, tempConfigStyleSheet.Definition(), tempConfigStyleSheet.InheritPermissions);

					addNewPolicies(newWebStyleSheet, tempConfigStyleSheet.ConfigPolicies());
					addNewProperty(newWebStyleSheet, tempConfigStyleSheet.ConfigProperties());
				}

				build(tempConfigFolder, newWebFolder);
			}
		}

		private static void addNewPolicies(WebItem webItem, IEnumerable<ConfigPolicy> configPolicies)
		{
			foreach (var configPolicy in configPolicies)
			{
				var roles = new Role[0];

				foreach (var configRole in configPolicy.ConfigRoles())
				{
					var role = new Role { Name = configRole.Name };

					roles = (Role[])Utilities.ArrayManipulation.Insert(roles, role);
				}

				webItem.AddNewPolicy(configPolicy.Name, roles);
			}
		}

		private static void addNewProperty(WebItem webItem, IEnumerable<ConfigProperty> configProperties)
		{
			foreach (var configProperty in configProperties)
			{
				webItem.AddNewProperty(configProperty.Name, configProperty.Value);
			}
		}
	}

	public class InputModel
	{
		public string ReportServerUrl { get; set; }
		public string PathToConfigurationFile { get; set; }
		public string ReportServerRootFolder { get; set; }
		public string ProjectName { get; set; }
	}
}
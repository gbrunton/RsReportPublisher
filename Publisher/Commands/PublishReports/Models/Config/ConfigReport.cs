using System;
using System.IO;
using System.Xml;

namespace Publisher.Commands.PublishReports.Models.Config
{
	public class ConfigReport : ConfigItem
	{
		//
		// Constants
		//

		public const string Report_Tag = "report";
		private const string commonReportsFolderName = "CommonReports";
		private const string reportFileExtension = ".rdl";
		private const string commonReport_Attribute = "commonReport";

		//
		// Fields
		//

		private object isCommonReport;

		//
		// Constructors
		//

		public ConfigReport(XmlNode node) : base(node) {}

		//
		// Public Properties/Methods
		//

		public bool IsCommonReport
		{
			get
			{
				if (this.isCommonReport == null)
				{
					var commonReport = base.node.Attributes[commonReport_Attribute];

					this.isCommonReport = !(commonReport != null) || Convert.ToBoolean(commonReport.Value);
				}

				return (bool)this.isCommonReport;
			}
		}

		public byte[] Definition()
		{
			byte[] definition;

			XmlNode configurationRoot = null;
			XmlNode folderRoot = null;
			var parentNode = base.node.ParentNode;

			while (true)
			{
				if (parentNode == null)
				{
					break;
				}
				
				if (parentNode.Name.Equals(ConfigFolder.Folder_Tag))
				{
					folderRoot = parentNode;
				}
				else if (parentNode.Name.Equals(ConfigConfiguration.Configuration_Tag))
				{
					configurationRoot = parentNode;
				}

				parentNode = parentNode.ParentNode;
			}

			var rootDirectory = commonReportsFolderName;

			if (! this.IsCommonReport)
			{
				rootDirectory = new ConfigFolder(folderRoot).Name;
			}

			var fullPath = string.Format(@"{0}\{1}\{2}{3}",
			                             new object[]
			                             	{
			                             		new ConfigConfiguration(configurationRoot).ReportLocalPath, rootDirectory, base.Name,
			                             		reportFileExtension
			                             	});

			using (var fileStream = File.OpenRead(fullPath))
			{
				var fileStreamLength = Convert.ToInt32(fileStream.Length);
				definition = new byte[fileStreamLength];

				fileStream.Read(definition, 0, fileStreamLength);
			}			

			return definition;
		}
	}
}
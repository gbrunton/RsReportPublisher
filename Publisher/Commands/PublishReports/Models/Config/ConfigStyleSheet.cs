using System;
using System.IO;
using System.Xml;

namespace Publisher.Commands.PublishReports.Models.Config
{
	public class ConfigStyleSheet : ConfigItem
	{
		//
		// Constants
		//

		public const string StyleSheet_Tag = "styleSheet";
		private const string commonReportsFolderName = "CommonReports";
		private const string common_Attribute = "commonReport";
		
		//
		// Fields
		//

		private object isCommon;

		//
		// Constructors
		//

		public ConfigStyleSheet(XmlNode node) : base(node) {}

		//
		// Public Properties/Methods
		//

		public bool IsCommonReport
		{
			get
			{
				if (this.isCommon == null)
				{
					var commonReport = base.node.Attributes[common_Attribute];

					this.isCommon = !(commonReport != null) || Convert.ToBoolean(commonReport.Value);
				}

				return (bool)this.isCommon;
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

			var fullPath = string.Format(@"{0}\{1}\{2}",
			                             new object[]
			                             	{
			                             		new ConfigConfiguration(configurationRoot).ReportLocalPath, rootDirectory, base.Name
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
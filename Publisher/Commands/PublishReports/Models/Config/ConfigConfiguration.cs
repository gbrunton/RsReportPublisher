using System;
using System.IO;
using System.Xml;

namespace Publisher.Commands.PublishReports.Models.Config
{
	public class ConfigConfiguration : ConfigItem
	{
		//
		// Constants
		//

		private const string reportPublisherFileName = "ReportPublisher.config";
		public const string Configuration_Tag = "configuration";
		private const string reportLocalPath_Attribute = "reportLocalPath";

		//
		// Fields
		//

		private XmlDocument configFile;
		private ConfigFolder[] configFolders;
		private string reportLocalPath;

		//
		// Constructors
		//

		public ConfigConfiguration()
		{
			this.configFile = new XmlDocument();
			this.configFile.Load(File.OpenText(string.Format(@"{0}..\Configuration\{1}", AppDomain.CurrentDomain.BaseDirectory, reportPublisherFileName)));

			base.node = this.configFile.DocumentElement;
		}

		public ConfigConfiguration(XmlNode node) : base(node) {}

		//
		// Public Properties/Methods
		//

		public ConfigFolder[] Folders()
		{
			if (this.configFolders == null)
			{
				this.configFolders = new ConfigFolder[0];

				var nodes = base.node.SelectNodes(ConfigFolder.Folder_Tag);

				if (nodes == null) throw new NullReferenceException();

				foreach (XmlNode selectedNode in nodes)
				{
					this.configFolders = (ConfigFolder[])Utilities.ArrayManipulation.Insert(this.configFolders, new ConfigFolder(selectedNode));
				}
			}

			return this.configFolders;
		}

		public string ReportLocalPath
		{
			get
			{
				if (this.reportLocalPath == null)
				{
					var reportLocalPathFromAttributes = base.node.Attributes[reportLocalPath_Attribute];

					if (reportLocalPathFromAttributes != null)
					{
						this.reportLocalPath = reportLocalPathFromAttributes.Value;
					}
				}

				return this.reportLocalPath;
			}
		}
	}
}
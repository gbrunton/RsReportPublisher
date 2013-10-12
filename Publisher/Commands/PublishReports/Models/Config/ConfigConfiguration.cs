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

		public const string Configuration_Tag = "configuration";

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
			this.configFile.Load(StaticVariables.PathToConfigurationFile);
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
					this.reportLocalPath = Path.GetDirectoryName(StaticVariables.PathToConfigurationFile);
				}

				return this.reportLocalPath;
			}
		}
	}
}
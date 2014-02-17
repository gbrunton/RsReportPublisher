using System;
using System.Xml;

namespace Publisher.Commands.PublishReports.Models.Config
{
	public class ConfigFolder : ConfigItem
	{
		//
		// Constants
		//

		public const string Folder_Tag = "folder";

		//
		// Fields
		//

		private ConfigFolder[] configFolders;
		private ConfigReport[] configReports;
		private ConfigStyleSheet[] configStyleSheets;
		private ConfigDataSource[] dataSources;

		//
		// Constructors
		//

		public ConfigFolder(XmlNode node) : base(node) {}

		//
		// Public Properties/Methods
		//

		public ConfigFolder[] Folders()
		{
			if (this.configFolders == null)
			{
				this.configFolders = new ConfigFolder[0];

				var nodes = base.node.SelectNodes(Folder_Tag);

				if (nodes == null) throw new NullReferenceException();

				foreach (XmlNode selectedNode in nodes)
				{
					this.configFolders = (ConfigFolder[])Utilities.ArrayManipulation.Insert(this.configFolders, new ConfigFolder(selectedNode));
				}
			}

			return this.configFolders;
		}

		public ConfigReport[] Reports()
		{
			if (this.configReports == null)
			{
				this.configReports = new ConfigReport[0];

				var nodes = base.node.SelectNodes(ConfigReport.Report_Tag);

				if (nodes == null) throw new NullReferenceException();

				foreach (XmlNode selectedNode in nodes)
				{
					this.configReports = (ConfigReport[])Utilities.ArrayManipulation.Insert(this.configReports, new ConfigReport(selectedNode));
				}
			}

			return this.configReports;
		}

		public ConfigStyleSheet[] StyleSheets()
		{
			if (this.configStyleSheets == null)
			{
				this.configStyleSheets = new ConfigStyleSheet[0];

				var nodes = base.node.SelectNodes(ConfigStyleSheet.StyleSheet_Tag);

				if (nodes == null) throw new NullReferenceException();

				foreach (XmlNode selectedNode in nodes)
				{
					this.configStyleSheets = (ConfigStyleSheet[])Utilities.ArrayManipulation.Insert(this.configStyleSheets, new ConfigStyleSheet(selectedNode));
				}
			}

			return this.configStyleSheets;
		}

		public ConfigDataSource[] DataSources
		{
			get
			{
				if (this.dataSources == null)
				{
					this.dataSources = new ConfigDataSource[0];

					var nodes = base.node.SelectNodes(ConfigDataSource.SharedDataSource_Tag);

					if (nodes == null) throw new NullReferenceException();

					foreach (XmlNode selectedNode in nodes)
					{
						this.dataSources = (ConfigDataSource[])Utilities.ArrayManipulation.Insert(this.dataSources, new ConfigDataSource(selectedNode));
					}
				}

				return this.dataSources;
			}
		}
	}
}
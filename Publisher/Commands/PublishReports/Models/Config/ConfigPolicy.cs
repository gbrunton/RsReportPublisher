using System;
using System.Xml;

namespace Publisher.Commands.PublishReports.Models.Config
{
	public class ConfigPolicy : ConfigItem
	{
		//
		// Constants
		//

		public const string Policy_Tag = "policy";

		//
		// Fields
		//

		private ConfigRole[] configRoles;

		//
		// Constructors
		//

		public ConfigPolicy(XmlNode node) : base(node) {}

		//
		// Public Properties/Methods
		//

		public ConfigRole[] ConfigRoles()
		{
			if (this.configRoles == null)
			{
				this.configRoles = new ConfigRole[0];

				var nodes = base.node.SelectNodes(ConfigRole.Role_Tag);

				if (nodes == null) throw new NullReferenceException();

				foreach (XmlNode selectedNode in nodes)
				{
					this.configRoles = (ConfigRole[])Utilities.ArrayManipulation.Insert(this.configRoles, new ConfigRole(selectedNode));
				}
			}

			return this.configRoles;
		}
	}
}
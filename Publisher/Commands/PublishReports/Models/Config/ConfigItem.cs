using System;
using System.Xml;

namespace Publisher.Commands.PublishReports.Models.Config
{
	public abstract class ConfigItem
	{
		//
		// Constants
		//

		protected const string name_Attribute = "name";
	    protected const string inheritPermissions_Attribute = "inheritPermissions";
		//
		// Fields
		//

		protected string localPath;
		protected string configItemName;
		protected XmlNode node;
		private ConfigPolicy[] configPolicies;
		private ConfigReportParameter[] configReportParameters;
		private ConfigProperty[] configProperties;
        protected object configItemInheritPermissions;

	    //
		// Constructors
		//

		public ConfigItem()	{}

		public ConfigItem(XmlNode node)
		{
			this.node = node;
		}

		//
		// Public Properties/Methods
		//

        public bool InheritPermissions
        {
            get
            {
                if (this.configItemInheritPermissions == null)
                {
                    var inheritPermissionsAttribute = this.node.Attributes[inheritPermissions_Attribute];

                    this.configItemInheritPermissions = inheritPermissionsAttribute == null || Convert.ToBoolean(inheritPermissionsAttribute.Value);
                }

                return (bool)this.configItemInheritPermissions;
            }
        }

		public string Name
		{
			get
			{
				if (this.configItemName == null)
				{
					var nameAttribute = this.node.Attributes[name_Attribute];

					if (nameAttribute != null)
					{
						this.configItemName = nameAttribute.Value;
					}
				}

				return this.configItemName;
			}
		}

		public bool Exists
		{
			get
			{
				return this.node != null;
			}
		}

		public ConfigPolicy[] ConfigPolicies()
		{
			if (this.configPolicies == null)
			{
				this.configPolicies = new ConfigPolicy[0];

				var nodes = this.node.SelectNodes(ConfigPolicy.Policy_Tag);

				if (nodes == null) throw new NullReferenceException();

				foreach (XmlNode selectedNode in nodes)
				{
					this.configPolicies =
						(ConfigPolicy[]) Utilities.ArrayManipulation.Insert(this.configPolicies, new ConfigPolicy(selectedNode));
				}
			}

			return this.configPolicies;
		}

		
		public ConfigReportParameter[] ConfigReportParameters()
		{
			if (this.configReportParameters == null)
			{
				this.configReportParameters = new ConfigReportParameter[0];

				var nodes = this.node.SelectNodes(ConfigReportParameter.Parameter_Tag);

				if (nodes == null) throw new NullReferenceException();

				foreach (XmlNode selectedNode in nodes)
				{
					this.configReportParameters = (ConfigReportParameter[])Utilities.ArrayManipulation.Insert(this.configReportParameters, new ConfigReportParameter(selectedNode));
				}
			}

			return this.configReportParameters;
		}

		public ConfigProperty[] ConfigProperties()
		{
			if (this.configProperties == null)
			{
				this.configProperties = new ConfigProperty[0];

				var nodes = this.node.SelectNodes(ConfigProperty.Property_Tag);

				if (nodes == null) throw new NullReferenceException();

				foreach (XmlNode selectedNode in nodes)
				{
					this.configProperties = (ConfigProperty[])Utilities.ArrayManipulation.Insert(this.configProperties, new ConfigProperty(selectedNode));
				}
			}

			return this.configProperties;
		}
	}
}
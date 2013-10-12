using System.Xml;

namespace Publisher.Commands.PublishReports.Models.Config
{
	public class ConfigReportParameter : ConfigItem
	{
		//
		// Constants
		//

		public const string Parameter_Tag = "parameter";
		private const string value_Attribute = "value";

		//
		// Fields
		//

		private string value;

		//
		// Constructors
		//

		public ConfigReportParameter(XmlNode node) : base(node) {}

		//
		// Public Properties/Methods
		//

		public string Value
		{
			get
			{
				if (this.value == null)
				{
					this.value = this.node.Attributes[value_Attribute].Value;
				}

				return this.value;
			}
		}
	}
}
using System.Xml;

namespace Publisher.Commands.PublishReports.Models.Config
{
	public class ConfigRole : ConfigItem
	{
		//
		// Constants
		//

		public const string Role_Tag = "role";

		//
		// Constructors
		//

		public ConfigRole(XmlNode node) : base(node) {}
	}
}
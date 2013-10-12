using System.Xml;

namespace Publisher.Commands.PublishReports.Models.Config
{
	public class ConfigDataSource : ConfigItem
	{
		//
		// Constants
		//

		public const string SharedDataSource_Tag = "sharedDataSource";

		//
		// Constructors
		//

		public ConfigDataSource(XmlNode node) : base(node) {}
	}
}
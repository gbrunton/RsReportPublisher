using System;
using Publisher.WebReportService;

namespace Publisher.Commands.PublishReports.Models.Web
{
	public class WebStyleSheet : WebItem
	{
		//
		// Fields
		//

		private readonly byte[] reportDefinition;
		
		//
		// Private/Protected Methods
		//


		//
		// Constructors
		//

		public WebStyleSheet(WebFund webFund, WebFolder parentWebFolder, string styleSheetName, byte[] reportDefinition, bool inheritPermissions)
            : base(webFund, parentWebFolder, styleSheetName, inheritPermissions)	
		{
			this.reportDefinition = reportDefinition;
		}

		//
		// Public Properties/Methods
		//

		public override void Save()
		{
			Console.WriteLine("Publishing Style Sheet {0}.", base.fullName);

			if (! base.exists)
			{
				base.reportingService.CreateResource(base.Name, base.ParentWebFolderFullName, true, reportDefinition, "text/plain", new Property[0]);
			}
			else
			{
				base.reportingService.SetResourceContents(base.fullName, reportDefinition, "text/plain");
			}

			base.savePolicies();
			base.saveProperties();
		}
	}
}
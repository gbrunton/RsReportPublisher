using System;
using Publisher.WebReportService;

namespace Publisher.Commands.PublishReports.Models.Web
{
	public class WebDataSource : WebItem
	{
		//
		// Constants
		//

		private const string connectionString = "";

		//
		// Fields
		//

		private readonly string dataSourceName;
		private readonly string databaseName;

		//
		// Constructors
		//

		public WebDataSource(WebFund webFund, WebFolder parentWebFolder, string dataSourceName, string databaseName, bool inheritPermissions)
            : base(webFund, parentWebFolder, dataSourceName, inheritPermissions) 
		{
			this.dataSourceName = dataSourceName;
			this.databaseName = databaseName;
		}

		//
		// Public Properties/Methods
		//

		public override void Save()
		{
			Console.WriteLine("Saving DataSource {0}.", base.fullName);

			if (! base.exists)
			{
				var definition = new DataSourceDefinition
				                 	{
				                 		CredentialRetrieval = CredentialRetrievalEnum.Integrated,
				                 		ConnectString = string.Format(connectionString, this.databaseName),
				                 		Enabled = true,
				                 		EnabledSpecified = true,
				                 		Extension = "SQL",
				                 		ImpersonateUser = false,
				                 		ImpersonateUserSpecified = true,
				                 		Prompt = null,
				                 		WindowsCredentials = false
				                 	};

				base.reportingService.CreateDataSource(this.Name, base.ParentWebFolderFullName, true, definition, new Property[0]);
			}

			var dataSourceReference = new DataSourceReference {Reference = base.fullName};

			this.DataSource = new DataSource {Name = this.dataSourceName, Item = dataSourceReference};
		}

		public DataSource DataSource { get; private set; }
	}
}
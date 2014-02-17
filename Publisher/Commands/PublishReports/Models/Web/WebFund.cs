using System;
using System.Web.Services.Protocols;

namespace Publisher.Commands.PublishReports.Models.Web
{
	public class WebFund
	{
		//
		// Fields
		//

		private readonly string name;
		private readonly WebFolder parentWebFolder;
		private readonly WebFolder webFolder;

		//
		// Constructors
		//

		public WebFund(string rootWebFolderName, string webFundName, bool inheritPermissions, params string[] dataSourceName)
		{
			this.name = webFundName;

			this.parentWebFolder = new WebFolder(this, rootWebFolderName) {DeleteExistingFolders = false};
            this.webFolder = this.parentWebFolder.AddWebFolder(webFundName, inheritPermissions);
			this.webFolder.SetDataSources(dataSourceName, inheritPermissions);
		}

		//
		// Public Properties/Methods
		//

		public void Save()
		{
			var reportingService = this.parentWebFolder.ReportingService;

			try
			{
				this.parentWebFolder.Save();
			}
			catch (SoapException ex)
			{
				Console.WriteLine(ex.Detail.InnerText.ToCharArray());
			}
			finally
			{
				reportingService.BatchHeaderValue = null;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public WebFolder RootWebFolder
		{
			get
			{
				return this.webFolder;
			}
		}

		public WebDataSource[] GetDataSources()
		{
			return this.webFolder.GetDataSources();
		}
	}
}
using System;
using System.Collections.Generic;
using Publisher.WebReportService;

namespace Publisher.Commands.PublishReports.Models.Web
{
	public class WebFolder : WebItem
	{
		//
		// Fields
		//

		private WebFolder[] webFolders = new WebFolder[0];
		private WebReport[] webReports = new WebReport[0];
		private WebStyleSheet[] webStyleSheets = new WebStyleSheet[0];
		private WebDataSource webDataSource;
		private bool deleteExistingFolders = true;

		//
		// Constructors
		//

		public WebFolder(WebFund webFund, string folderName) : base(webFund, folderName) 
		{
			base.exists = base.ExistsOnWeb(this.ParentWebFolderFullName);
		}

		public WebFolder(WebFund webFund, WebFolder parentWebFolder, string folderName, bool inheritPermissions) 
			: base(webFund, parentWebFolder, folderName, inheritPermissions) {}

		//
		// Private/Protected Methods
		//

		private bool isItemValid(string itemName)
		{
			var valid = false;

			if (this.webDataSource != null && this.webDataSource.Name.Equals(itemName))
			{
				valid = true;
			}

			if (! valid)
			{
				valid = isItemValid(itemName, this.webFolders);
			}

			if (! valid)
			{
				valid = isItemValid(itemName, this.webReports);
			}

			if (!valid)
			{
				valid = isItemValid(itemName, this.webStyleSheets);
			}

			return valid;
		}

		private static bool isItemValid(string itemName, IEnumerable<WebItem> webItems)
		{
			var valid = false;

			foreach (var item in webItems)
			{
				if (!item.Name.Equals(itemName)) continue;
				valid = true;
				break;
			}

			return valid;
		}

		//
		// Public Properties/Methods
		//

		public override void Save()
		{
			Console.WriteLine("Publishing Folder {0}.", base.fullName);

			if (! base.exists)
			{
				base.reportingService.CreateFolder(this.Name, base.ParentWebFolderFullName, new Property[0]);
			}
			else if (this.deleteExistingFolders)
			{
				foreach (var item in base.reportingService.ListChildren(base.fullName, false))
				{
					if (this.isItemValid(item.Name)) continue;
					Console.WriteLine("Deleting item {0}.", item.Path);
					base.reportingService.DeleteItem(item.Path);
				}
			}

			if (this.webDataSource != null)
			{
				this.webDataSource.Save();
			}

			foreach (var webReport in this.webReports)
			{
				webReport.Save();
			}

			foreach (var webStyleSheet in this.webStyleSheets)
			{
				webStyleSheet.Save();
			}

			foreach (var webFolder in this.webFolders)
			{
				webFolder.Save();
			}

			base.savePolicies();
			base.saveProperties();
		}

		public bool DeleteExistingFolders
		{
			set
			{
				this.deleteExistingFolders = value;
			}
		}

		public WebFolder AddWebFolder(string folderName, bool inheritPermissions)
		{
            var webFolder = new WebFolder(base.webFund, this, folderName, inheritPermissions);
			this.webFolders = (WebFolder[])Utilities.ArrayManipulation.Insert(this.webFolders, webFolder);
			return webFolder;
		}

        public WebReport AddWebReport(string reportName, byte[] reportDefinition, bool inheritPermissions)
		{
            var webReport = new WebReport(base.webFund, this, reportName, reportDefinition, inheritPermissions);
			this.webReports = (WebReport[])Utilities.ArrayManipulation.Insert(this.webReports, webReport);
			return webReport;
		}

        public WebStyleSheet AddWebStyleSheet(string styleSheetName, byte[] styleSheetDefinition, bool inheritPermissions)
		{
            var webStyleSheet = new WebStyleSheet(base.webFund, this, styleSheetName, styleSheetDefinition, inheritPermissions);
			this.webStyleSheets = (WebStyleSheet[])Utilities.ArrayManipulation.Insert(this.webStyleSheets, webStyleSheet);
			return webStyleSheet;
		}

        public void SetDataSource(string dataSourceName, bool inheritPermissions)
		{
            this.webDataSource = new WebDataSource(base.webFund, this, dataSourceName, base.webFund.Name, inheritPermissions);
		}

		public WebDataSource GetDataSource()
		{
			return this.webDataSource;
		}
	}
}
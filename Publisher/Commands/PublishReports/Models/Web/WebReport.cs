using System;
using Publisher.WebReportService;

namespace Publisher.Commands.PublishReports.Models.Web
{
	public class WebReport : WebItem
	{
		//
		// Fields
		//

		private readonly byte[] reportDefinition;
		private ReportParameter[] reportParameters;

		//
		// Private/Protected Methods
		//

		protected void saveReportParameters()
		{
			if (this.reportParameters != null)
			{
				this.reportingService.SetReportParameters(base.fullName, this.reportParameters);
			}
			else
			{
				this.reportingService.SetReportParameters(base.fullName, new ReportParameter[0]);
			}
		}

		//
		// Constructors
		//

        public WebReport(WebFund webFund, WebFolder parentWebFolder, string reportName, byte[] reportDefinition, bool inheritPermissions)
            : base(webFund, parentWebFolder, reportName, inheritPermissions)	
		{
			this.reportDefinition = reportDefinition;
		}

		//
		// Public Properties/Methods
		//

		public override void Save()
		{
			Console.WriteLine("Publishing Report {0}.", base.fullName);

			if (! base.exists)
			{
				base.reportingService.CreateReport(base.Name, base.ParentWebFolderFullName, true, reportDefinition, new Property[0]);
			}
			else
			{
				base.reportingService.SetReportDefinition(base.fullName, reportDefinition);
			}

			base.reportingService.SetItemDataSources(base.fullName, new[] {base.webFund.GetDataSource().DataSource});
			base.savePolicies();
			base.saveProperties();
			this.saveReportParameters();
		}

		public void AddNewReportParameter(string name, string value)
		{
			if (this.reportParameters == null)
			{
				this.reportParameters = new ReportParameter[0];
			}

			var reportParameter = new ReportParameter {Name = name, DefaultValues = new[] {value}};

			this.reportParameters = (ReportParameter[])Utilities.ArrayManipulation.Insert(this.reportParameters, reportParameter);
		}
	}
}
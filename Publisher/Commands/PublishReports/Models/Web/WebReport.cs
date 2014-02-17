using System;
using System.Linq;
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

			var dataSourcesAssociatedWithTheReport = base.reportingService.GetItemDataSources(base.fullName);
			var dataSources = base.webFund
				.GetDataSources()
				// Look here, I've never used a Join before now! Weird syntax I must say.
				.Join(dataSourcesAssociatedWithTheReport, ds => ds.Name, ids => ids.Name, (source, dataSource) => source)
				.Select(x => x.DataSource)
				.ToArray();

			if (dataSources.Any())
			{
				base.reportingService.SetItemDataSources(base.fullName, dataSources);				
			}

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
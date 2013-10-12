using System.Net;
using Publisher.WebReportService;

namespace Publisher.Commands.PublishReports.Models.Web
{
	public abstract class WebItem
	{
		//
		// Constants
		//

		private const string name_Property = "Name";
		private const string description_Property = "Description";
		private const string reportingServicesRootFolder = "/";

		//
		// Fields
		//

		protected readonly ReportingService2005 reportingService;
		protected readonly WebFolder parentWebFolder;
		protected readonly string fullName;
		protected readonly WebFund webFund;
		protected Policy[] policies;
		protected bool exists;
		protected Property[] properties;
	    private bool inheritPermissions;

	    //
		// Private/Protected Methods
		//

		protected void savePolicies()
		{
			if (this.policies != null)
			{
				this.reportingService.SetPolicies(this.fullName, this.policies);
			}
			else
			{
				if (this.exists && this.inheritPermissions)
				{
					bool inheritParent;

					this.reportingService.GetPolicies(this.fullName, out inheritParent);

					if (!inheritParent)
					{
						this.reportingService.InheritParentSecurity(this.fullName);
					}
				}
			}
		}

		protected void saveProperties()
		{
			if (this.properties != null)
			{
				this.reportingService.SetProperties(this.fullName, this.properties);
			}
			else
			{
				var property = new Property {Name = description_Property, Value = ""};
				this.reportingService.SetProperties(this.fullName, new[] {property});
			}
		}

		protected bool ExistsOnWeb(string webFolderFullName)
		{
			var existsOnWeb = false;
			var searchCondition = new SearchCondition
			                      	{
			                      		Condition = ConditionEnum.Contains,
			                      		ConditionSpecified = true,
			                      		Name = name_Property,
			                      		Value = this.Name
			                      	};

			var catalogItems = this.reportingService.FindItems
				(
				webFolderFullName,
				BooleanOperatorEnum.Or,
				new[] {searchCondition}
				);

			if (catalogItems != null && catalogItems.Length > 0)
			{
				foreach (var catalogItem in catalogItems)
				{
					if (!catalogItem.Path.Equals(this.fullName)) continue;
					existsOnWeb = true;
					break;
				}
			}

			return existsOnWeb;
		}

		private bool existsOnWeb(WebItem webFolder)
		{
			return webFolder.Exists && this.ExistsOnWeb(webFolder.FullName);
		}

		//
		// Constructors
		//

		public WebItem(WebFund webFund, string webItemName)
		{
			this.webFund = webFund;
			this.reportingService = new SslReportService
			                        	{
			                        		Credentials = CredentialCache.DefaultCredentials
			                        	};
			this.fullName = "/" + webItemName;
		}

		public WebItem(WebFund webFund, WebFolder parentWebFolder, string name, bool inheritPermissions)
		{
			this.webFund = webFund;
			this.reportingService = parentWebFolder.ReportingService;
			this.parentWebFolder = parentWebFolder;
			this.fullName = string.Format("{0}/{1}", parentWebFolder.FullName, name);
			this.exists = this.existsOnWeb(this.parentWebFolder);
		    this.inheritPermissions = inheritPermissions;
		}

		//
		// Public Properties/Methods
		//

		public string FullName
		{
			get
			{
				return this.fullName;
			}
		}

        public bool InheritPermissions
        {
            get
            {
                return this.inheritPermissions;
            }
        }

		public string Name
		{
			get
			{
				var parts = this.fullName.Split(new[] {'/'});
				return parts[parts.Length - 1];
			}
		}

		public ReportingService2005 ReportingService
		{
			get
			{
				return this.reportingService;
			}
		}

		public string ParentWebFolderFullName
		{
			get {
				return this.parentWebFolder != null ? this.parentWebFolder.FullName : reportingServicesRootFolder;
			}
		}

		public void AddNewPolicy(string groupUserName, Role[] roles)
		{
			if (this.policies == null)
			{
				this.policies = new Policy[0];
			}

			var policy = new Policy {GroupUserName = groupUserName, Roles = roles};
			this.policies = (Policy[])Utilities.ArrayManipulation.Insert(this.policies, policy);
		}

		public void AddNewProperty(string name, string value)
		{
			if (this.properties == null)
			{
				this.properties = new Property[0];
			}

			var property = new Property {Name = name, Value = value};
			this.properties = (Property[])Utilities.ArrayManipulation.Insert(this.properties, property);
		}

		public bool Exists
		{
			get
			{
				return this.exists;
			}
		}

		// 
		// Abstract methods that must be overridden
		//

		public abstract void Save();
	}
}
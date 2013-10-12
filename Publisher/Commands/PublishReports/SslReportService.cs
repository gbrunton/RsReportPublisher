using System.Net;
using Publisher.WebReportService;

namespace Publisher.Commands.PublishReports
{
	public class SslReportService : ReportingService2005
	{
		protected override WebRequest GetWebRequest(System.Uri uri)
		{
			var res = (HttpWebRequest)base.GetWebRequest(uri);
			res.SendChunked = true;
			return res;
		}
	}
}
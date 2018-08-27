using Umbraco.Core.Models;

namespace Our.Umbraco.MultiLingualUrls.Models
{
	public class DomainInfo
	{
		public IPublishedContent Node { get; set; }
		public int SegmentCount { get; set; }
	}
}

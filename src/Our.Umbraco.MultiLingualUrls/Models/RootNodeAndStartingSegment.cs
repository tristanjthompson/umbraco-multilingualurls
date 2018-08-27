using Umbraco.Core.Models;

namespace Our.Umbraco.MultiLingualUrls.Models
{
	public class RootNodeAndStartingSegment
	{
		public IPublishedContent RootNode { get; set; }
		public int StartingSegment { get; set; }
	}
}

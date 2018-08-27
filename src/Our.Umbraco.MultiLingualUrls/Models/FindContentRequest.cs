using System;
using Umbraco.Core.Models;

namespace Our.Umbraco.MultiLingualUrls.Models
{
	public class FindContentRequest
	{
		public bool HasDomain { get; set; }
		public IDomain Domain { get; set; }
		public Uri DomainUri { get; set; }
		public Uri CurrentUri { get; set; }
		public string CultureName { get; set; }
	}
}

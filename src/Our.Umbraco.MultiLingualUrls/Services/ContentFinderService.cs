using Our.Umbraco.MultiLingualUrls.Config;
using Our.Umbraco.MultiLingualUrls.Models;
using Our.Umbraco.MultiLingualUrls.Resolver;
using Our.Umbraco.MultiLingualUrls.Wrappers;
using System;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace Our.Umbraco.MultiLingualUrls.Services
{
	public class ContentFinderService
	{
		private readonly IConfig _config = ServiceResolver.GetService<IConfig>();
		private readonly IUmbracoWrapper _umbracoWrapper = ServiceResolver.GetService<IUmbracoWrapper>();

		public IPublishedContent FindContent(FindContentRequest request)
		{
			var domainInfo = DomainInfo(request);

			// iterate along the URL segments in tandem with the nodes, matching them to the segments
			var currentItem = domainInfo.Node;
			for (int i = domainInfo.SegmentCount; i < request.Uri.Segments.Length; i++)
			{
				var urlSegment = request.Uri.Segments[i].TrimEnd('/');
				var segmentFound = false;
				foreach (var node in  _umbracoWrapper.Children(currentItem))
				{
					var nodeUrlSegment = NodeUrlSegment(node, request.CultureName);

					if (urlSegment.InvariantEquals(nodeUrlSegment))
					{
						segmentFound = true;
						currentItem = node;
						break;
					}
				}

				if (!segmentFound)
					return null;
			}

			return currentItem;
		}

		public DomainInfo DomainInfo(FindContentRequest request)
		{
			return DomainInfo(request.HasDomain, request.Domain, request.DomainUri);
		}

		public DomainInfo DomainInfo(bool hasDomain, IDomain domain, Uri domainUri)
		{
			if (hasDomain && domain.RootContentId.HasValue)
			{
				return new DomainInfo
				{
					Node = _umbracoWrapper.TypedContent(domain.RootContentId.Value),
					SegmentCount = domainUri.Segments.Count()
				};
			}
			else
			{
				return new DomainInfo
				{
					Node = _umbracoWrapper.TypedContentAtRoot()?.FirstOrDefault(),
					SegmentCount = 1
				};
			}
		}

		public string NodeUrlSegment(IPublishedContent item, string cultureName)
		{
			var itemUrlSegment = _umbracoWrapper.MultiLingualUrlSegment(item, cultureName);

			if (string.IsNullOrEmpty(itemUrlSegment))
			{
				itemUrlSegment = _umbracoWrapper.DefaultUrlSegment(item);
			}

			return itemUrlSegment;
		}
	}
}

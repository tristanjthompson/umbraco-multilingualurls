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
			var rootNodeAndStartingSegment = RootNodeAndStartingSegment(request.HasDomain, request.Domain, request.DomainUri);

			// iterate along the URL segments in tandem with the nodes, matching them to the segments
			var currentItem = rootNodeAndStartingSegment.RootNode;
			for (int i = rootNodeAndStartingSegment.StartingSegment; i < request.CurrentUri.Segments.Length; i++)
			{
				var urlSegment = request.CurrentUri.Segments[i].TrimEnd('/');
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

		public RootNodeAndStartingSegment RootNodeAndStartingSegment(bool hasDomain, IDomain domain, Uri domainUri)
		{
			if (hasDomain && domain.RootContentId.HasValue)
			{
				return new RootNodeAndStartingSegment
				{
					RootNode = _umbracoWrapper.TypedContent(domain.RootContentId.Value),
					StartingSegment = domainUri.Segments.Count()
				};
			}
			else
			{
				return new RootNodeAndStartingSegment
				{
					RootNode = _umbracoWrapper.TypedContentAtRoot()?.FirstOrDefault(),
					StartingSegment = 1
				};
			}
		}

		public string NodeUrlSegment(IPublishedContent item, string cultureName)
		{
			var itemUrlSegment = MultiLingualUrlSegment(item, cultureName);

			if (string.IsNullOrEmpty(itemUrlSegment))
				itemUrlSegment = DefaultUrlSegment(item);

			return itemUrlSegment.ToUrlSegment();
		}

		public string MultiLingualUrlSegment(IPublishedContent item, string cultureName)
		{
			if (_umbracoWrapper.HasProperty(item, _config.VortoUrlAliasProperty))
			{
				return  _umbracoWrapper.GetVortoValue<string>(item, _config.VortoUrlAliasProperty, cultureName: cultureName);
			}
			else
			{
				return string.Empty;
			}
		}

		public string DefaultUrlSegment(IPublishedContent item)
		{
			return item.Name;
		}
	}
}

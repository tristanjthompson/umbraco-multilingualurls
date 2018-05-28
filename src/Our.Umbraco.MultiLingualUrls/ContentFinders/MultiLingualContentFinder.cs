using Our.Umbraco.MultiLingualUrls.Config;
using Our.Umbraco.Vorto.Extensions;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace Our.Umbraco.MultiLingualUrls.ContentFinders
{
	public class MultiLingualContentFinder : IContentFinder
	{
		private readonly IConfig _config;
		private readonly ICacheProvider _cache;

		public MultiLingualContentFinder(IConfig config = null, ICacheProvider cache = null)
		{
			_config = config;
			_cache = cache ?? UmbracoContext.Current.Application.ApplicationCache.RequestCache;
		}

		public bool TryFindContent(PublishedContentRequest contentRequest)
		{
			// TODO: Caching
			// TODO: move out to a test-able service
			if (contentRequest == null)
				return false;

			if (contentRequest.Uri.Segments.Length <= 1)
				return true;

			var helper = new UmbracoHelper(UmbracoContext.Current);

			// try to find the matching root node
			IPublishedContent root = null;
			int startingSegment;
			if (contentRequest.HasDomain)
			{
				root = helper.TypedContent(contentRequest.UmbracoDomain.RootContentId);
				startingSegment = contentRequest.DomainUri.Segments.Count();
			}
			else
			{
				root = helper.TypedContentAtRoot().FirstOrDefault();
				startingSegment = 1;
			}

			// iterate along the segments while iterating down the content tree to find multi-lingual values
			var currentItem = root;
			for (int i = startingSegment; i < contentRequest.Uri.Segments.Length; i++)
			{
				// check vorto values
				var segmentToCheck = contentRequest.Uri.Segments[i].TrimEnd('/');
				bool segmentFound = false;
				foreach (var item in currentItem.Children)
				{

					string itemSegment = "";
					if (item.HasProperty(_config.VortoUrlAliasProperty))
					{
						itemSegment = item.GetVortoValue<string>(_config.VortoUrlAliasProperty, cultureName: contentRequest.Culture.Name);
					}

					if (string.IsNullOrEmpty(itemSegment))
						itemSegment = item.Name;

					itemSegment = itemSegment.ToUrlSegment();

					if (segmentToCheck.InvariantEquals(itemSegment))
					{
						segmentFound = true;
						currentItem = item;
						break;
					}
				}

				if (!segmentFound)
					return false;
			}

			contentRequest.PublishedContent = currentItem;
			return true;
		}
	}
}

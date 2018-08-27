using Our.Umbraco.MultiLingualUrls.Models;
using Our.Umbraco.MultiLingualUrls.Resolver;
using Our.Umbraco.MultiLingualUrls.Services;
using Umbraco.Web.Routing;

namespace Our.Umbraco.MultiLingualUrls.ContentFinders
{
	public class MultiLingualContentFinder : IContentFinder
	{
		private readonly ContentFinderService _multiLingualUrlService = ServiceResolver.GetService<ContentFinderService>();

		public bool TryFindContent(PublishedContentRequest contentRequest)
		{
			if (contentRequest == null)
				return false;

			if (contentRequest.Uri.Segments.Length <= 1)
				return true;

			// TODO: caching?
			var foundNode = _multiLingualUrlService.FindContent(new FindContentRequest
			{
				HasDomain = contentRequest.HasDomain,
				Domain = contentRequest.UmbracoDomain,
				DomainUri = contentRequest.DomainUri,
				Uri = contentRequest.Uri,
				CultureName = contentRequest.Culture.Name
			});

			if (foundNode != null)
			{
				contentRequest.PublishedContent = foundNode;
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}

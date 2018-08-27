using Our.Umbraco.MultiLingualUrls.Resolver;
using Our.Umbraco.MultiLingualUrls.Wrappers;
using Umbraco.Core.Models;

namespace Our.Umbraco.MultiLingualUrls.Services
{
	public class MultiLingualUrlProviderService
	{
		private readonly IUmbracoWrapper _umbracoWrapper = ServiceResolver.GetService<IUmbracoWrapper>();

		public string NodeSegment(IPublishedContent node)
		{
			string segment = string.Empty;
			if (!_umbracoWrapper.IsHomePage(node) && !_umbracoWrapper.IsRootNode(node))
			{
				segment = _umbracoWrapper.MultiLingualUrlSegment(node, null);
			}

			if (string.IsNullOrWhiteSpace(segment))
			{
				segment = _umbracoWrapper.DefaultUrlSegment(node);
			}

			return segment;
		}
	}
}

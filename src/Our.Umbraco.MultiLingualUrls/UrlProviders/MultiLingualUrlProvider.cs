using System;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Umbraco.Core.Configuration;
using Our.Umbraco.MultiLingualUrls.Resolver;
using Our.Umbraco.MultiLingualUrls.Services;
using Our.Umbraco.MultiLingualUrls.Wrappers;

namespace Our.Umbraco.MultiLingualUrls.UrlProviders
{
	public class MultiLingualUrlProvider : DefaultUrlProvider, IUrlProvider
	{
		private readonly IUmbracoWrapper _umbracoWrapper = ServiceResolver.GetService<IUmbracoWrapper>();
		private readonly MultiLingualUrlProviderService _multiLingualUrlProviderService = ServiceResolver.GetService<MultiLingualUrlProviderService>();

		public MultiLingualUrlProvider()
			: base(UmbracoConfig.For.UmbracoSettings().RequestHandler)
		{
		}

		public new string GetUrl(UmbracoContext umbracoContext, int id, Uri current, UrlProviderMode mode)
		{
			var node = _umbracoWrapper.TypedContent(id);	

			if (_umbracoWrapper.IsRootNode(node))
				return base.GetUrl(umbracoContext, id, current, mode);

			var segment = _multiLingualUrlProviderService.NodeSegment(node);

			return ParentUrl(umbracoContext, node.Parent.Id, current, mode) 
				+ segment.EnsureEndsWith('/');
		}

		private string ParentUrl(UmbracoContext umbracoContext, int parentId, Uri current, UrlProviderMode mode)
		{
			return GetUrl(umbracoContext, parentId, current, mode).EnsureEndsWith('/');
		}
	}
}
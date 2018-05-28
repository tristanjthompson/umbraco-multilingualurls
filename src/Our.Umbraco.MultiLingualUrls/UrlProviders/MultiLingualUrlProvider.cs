using Our.Umbraco.MultiLingualUrls.Config;
using Our.Umbraco.Vorto.Extensions;
using System;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Umbraco.Core.Configuration;

namespace Our.Umbraco.MultiLingualUrls.UrlProviders
{
	public class MultiLingualUrlProvider : DefaultUrlProvider
	{
		private readonly IConfig _config;
		private readonly ICacheProvider _cache;

		public MultiLingualUrlProvider(IConfig config = null, ICacheProvider cache = null)
			: base(UmbracoConfig.For.UmbracoSettings().RequestHandler)
		{
			_config = config;
			_cache = cache ?? UmbracoContext.Current.Application.ApplicationCache.RequestCache;
		}

		public new string GetUrl(UmbracoContext umbracoContext, int id, Uri current, UrlProviderMode mode)
		{
			if (IsSiteRootNode(umbracoContext, id, out IPublishedContent content))
				return base.GetUrl(umbracoContext, id, current, mode);

			return GetMultiLingualUrl(content, umbracoContext, current, mode);
		}

		private string GetMultiLingualUrl(IPublishedContent content, UmbracoContext umbracoContext, Uri current, UrlProviderMode mode)
		{
			string segment = string.Empty;
			if (!_config.HomePageDocumentTypeAliases.Any(a => content.DocumentTypeAlias.InvariantEquals(a)))
			{
				if (HasVortoUrlAliasProperty(content, _config.VortoUrlAliasProperty))
				{
					segment = content.GetVortoValue<string>(_config.VortoUrlAliasProperty, fallbackCultureName: _config.FallbackCultureName);

					if (string.IsNullOrWhiteSpace(segment))
					{
						segment = GetDefaultUrlSegment(content);
					}
					else
					{
						segment = segment.ToUrlSegment();
					}
				}
				else
				{
					segment = GetDefaultUrlSegment(content);
				}
			}

			return string.Concat(
					ParentUrl(umbracoContext, content.Parent.Id, current, mode)
					, segment
				)
				.EnsureEndsWith('/');
		}

		private bool IsSiteRootNode(UmbracoContext umbracoContext, int id, out IPublishedContent content)
		{
			// don't interfere with the root node url
			content = umbracoContext.ContentCache.GetById(id);
			return content.Parent == null;
		}

		private bool HasVortoUrlAliasProperty(IPublishedContent content, string alias)
		{
			return content.HasProperty(alias)
				&& content.HasVortoValue(alias);
		}

		private string GetDefaultUrlSegment(IPublishedContent content)
		{
			return content.GetPropertyValue<string>(_config.DefaultUrlAliasProperty, content.Name)
				.ToUrlSegment();
		}

		private string ParentUrl(UmbracoContext umbracoContext, int parentId, Uri current, UrlProviderMode mode)
		{
			return GetUrl(umbracoContext, parentId, current, mode).EnsureEndsWith('/');
		}
	}
}
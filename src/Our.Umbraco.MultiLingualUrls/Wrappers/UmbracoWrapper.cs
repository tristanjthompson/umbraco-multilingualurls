using Our.Umbraco.MultiLingualUrls.Config;
using Our.Umbraco.MultiLingualUrls.Resolver;
using Our.Umbraco.Vorto.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Our.Umbraco.MultiLingualUrls.Wrappers
{
	public class UmbracoWrapper : IUmbracoWrapper
	{
		private UmbracoHelper _helper => new UmbracoHelper(UmbracoContext.Current);
		private IConfig _config = ServiceResolver.GetService<IConfig>();

		public bool HasProperty(IPublishedContent content, string alias)
		{
			return content.HasProperty(alias);
		}

		public bool HasVortoProperty(IPublishedContent content, string alias)
		{
			return content.HasProperty(alias)
				&& content.HasVortoValue(alias);
		}

		public IPublishedContent TypedContent(int id)
		{
			return _helper.TypedContent(id);
		}

		public IEnumerable<IPublishedContent> TypedContentAtRoot()
		{
			return _helper.TypedContentAtRoot();
		}

		public T GetPropertyValue<T>(IPublishedContent content, string alias, T defaultValue = default(T), bool recurse = false)
		{
			return content.GetPropertyValue<T>(alias, recurse, defaultValue);
		}

		public T GetVortoValue<T>(IPublishedContent content, string propertyAlias, string cultureName = null, bool recursive = false, T defaultValue = default(T), string fallbackCultureName = null)
		{
			if (!HasVortoProperty(content, propertyAlias))
				return defaultValue;

			return content.GetVortoValue<T>(propertyAlias, cultureName, recursive, defaultValue, fallbackCultureName);
		}

		public IEnumerable<IPublishedContent> Children(IPublishedContent content)
		{
			return content.Children;
		}

		public bool IsHomePage(IPublishedContent node)
		{
			return _config.HomePageDocumentTypeAliases.Any(a => node.DocumentTypeAlias.InvariantEquals(a));
		}

		public bool IsRootNode(IPublishedContent content)
		{
			return content?.Parent == null;
		}

		public string MultiLingualUrlSegment(IPublishedContent item, string cultureName)
		{
			if (HasProperty(item, _config.VortoUrlAliasProperty))
			{
				var segment = GetVortoValue<string>(item, _config.VortoUrlAliasProperty, cultureName: cultureName);

				if (!string.IsNullOrEmpty(segment))
					return segment.ToUrlSegment();
			}

			return string.Empty;
		}

		public string DefaultUrlSegment(IPublishedContent item)
		{
			if (IsRootNode(item) || IsHomePage(item))
				return string.Empty;

			return item.Name.ToUrlSegment();
		}
	}
}

using Our.Umbraco.Vorto.Extensions;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Our.Umbraco.MultiLingualUrls.Wrappers
{
	public class UmbracoWrapper : IUmbracoWrapper
	{
		private UmbracoHelper _helper => new UmbracoHelper(UmbracoContext.Current);

		public bool HasProperty(IPublishedContent content, string alias)
		{
			return content.HasProperty(alias);
		}

		public IPublishedContent TypedContent(int id)
		{
			return _helper.TypedContent(id);
		}

		public IEnumerable<IPublishedContent> TypedContentAtRoot()
		{
			return _helper.TypedContentAtRoot();
		}

		public T GetVortoValue<T>(IPublishedContent content, string propertyAlias, string cultureName = null, bool recursive = false, T defaultValue = default(T), string fallbackCultureName = null)
		{
			if (!content.HasValue(propertyAlias) || !content.HasVortoValue(propertyAlias))
				return defaultValue;

			return content.GetVortoValue<T>(propertyAlias, cultureName, recursive, defaultValue, fallbackCultureName);
		}

		public IEnumerable<IPublishedContent> Children(IPublishedContent content)
		{
			return content.Children;
		}
	}
}

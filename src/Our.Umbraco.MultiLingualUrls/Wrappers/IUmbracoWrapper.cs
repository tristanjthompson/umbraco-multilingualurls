using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Our.Umbraco.MultiLingualUrls.Wrappers
{
	public interface IUmbracoWrapper
	{
		bool HasProperty(IPublishedContent content, string alias);
		IPublishedContent TypedContent(int nodeId);
		IEnumerable<IPublishedContent> TypedContentAtRoot();
		T GetVortoValue<T>(IPublishedContent content, string propertyAlias, string cultureName = null, bool recursive = false, T defaultValue = default(T), string fallbackCultureName = null);
		IEnumerable<IPublishedContent> Children(IPublishedContent content);
	}
}

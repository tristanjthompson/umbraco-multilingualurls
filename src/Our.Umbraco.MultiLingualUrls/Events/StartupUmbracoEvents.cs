using Our.Umbraco.MultiLingualUrls.ContentFinders;
using Our.Umbraco.MultiLingualUrls.UrlProviders;
using Umbraco.Core;
using Umbraco.Web.Routing;

namespace Our.Umbraco.MultiLingualUrls.Events
{
	public class StartupUmbracoEvents : ApplicationEventHandler
	{
		protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
		{
			UrlProviderResolver.Current.InsertTypeBefore<DefaultUrlProvider, MultiLingualUrlProvider>();
			ContentFinderResolver.Current.AddType<MultiLingualContentFinder>();

			// TODO - when adding caching, clear cache when publishing
		}
	}
}

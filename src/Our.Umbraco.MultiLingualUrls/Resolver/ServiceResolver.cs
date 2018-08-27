using Our.Umbraco.MultiLingualUrls.Config;
using Our.Umbraco.MultiLingualUrls.Services;
using Our.Umbraco.MultiLingualUrls.Wrappers;
using System.Web.Mvc;
using Umbraco.Core.Cache;
using Umbraco.Web;

namespace Our.Umbraco.MultiLingualUrls.Resolver
{
	public static class ServiceResolver
	{
		public static T GetService<T>()
			where T : class
		{
			return DependencyResolver.Current.GetService<T>() 
				?? (T) InternalGetService<T>();
		}

		private static T InternalGetService<T>()
			where T : class
		{
			if(typeof(T) == typeof(IConfig))
				return new Config.Config() as T;

			if (typeof(T) == typeof(ICacheProvider))
				return UmbracoContext.Current.Application.ApplicationCache.RequestCache as T;

			if (typeof(T) == typeof(IUmbracoWrapper))
				return new UmbracoWrapper() as T;

			if (typeof(T) == typeof(ContentFinderService))
				return new ContentFinderService() as T;

			if (typeof(T) == typeof(MultiLingualUrlProviderService))
				return new MultiLingualUrlProviderService() as T;
			
			return null;
		}
	}
}

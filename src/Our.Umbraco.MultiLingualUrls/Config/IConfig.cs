using System.Collections.Generic;

namespace Our.Umbraco.MultiLingualUrls.Config
{
	public interface IConfig
	{
		List<string> HomePageDocumentTypeAliases { get; }
		string VortoUrlAliasProperty { get; }
		string DefaultUrlAliasProperty { get; }
		string FallbackCultureName { get; }
	}
}

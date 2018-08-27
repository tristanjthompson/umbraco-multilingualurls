using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;

namespace Our.Umbraco.MultiLingualUrls.Config
{
	public class Config : IConfig
	{
		private const string ConfigPrefix = "Our.Umbraco.MultiLingualUrls:";
		public List<string> HomePageDocumentTypeAliases => GetConfigStringListValue(ConfigPrefix + nameof(VortoUrlAliasProperty));

		public string VortoUrlAliasProperty => ConfigurationManager.AppSettings[ConfigPrefix + nameof(VortoUrlAliasProperty)] ?? "vortoUrlAlias";

		public string FallbackCultureName => ConfigurationManager.AppSettings[ConfigPrefix + nameof(FallbackCultureName)] ?? "en-GB";

		private List<string> GetConfigStringListValue(string key)
		{
			var csvList = ConfigurationManager.AppSettings[key] ?? "";
			if (string.IsNullOrWhiteSpace(csvList))
				return new List<string>();

			return csvList
				.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(x => x.Trim())
				.ToList();
		}
	}
}

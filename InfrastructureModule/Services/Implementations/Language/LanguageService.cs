using System.Collections.Generic;
using InfrastructureModule.Models.Components.Navigation;
using InfrastructureModule.Services.Interfaces.Language;
using Sitecore.Data.Managers;

namespace InfrastructureModule.Services.Implementations.Language
{
	public class LanguageService : ILanguageService
	{
		public Languages GetAllLanguages()
		{
			var mappedLanguageModel = new Languages();
			var mappedLanguageCollection = new List<InfrastructureModule.Models.Components.Navigation.Language>();
			var sitecoreLanguageCollection = LanguageManager.GetLanguages(Sitecore.Context.Database);

			var currentLanguageName = Sitecore.Context.Item.Language.Name;

			foreach (var sitecoreLanguage in sitecoreLanguageCollection)
			{
				var languageName = sitecoreLanguage.Name;

				var language = new InfrastructureModule.Models.Components.Navigation.Language()
				{
					Key = sitecoreLanguage.CultureInfo.NativeName,
					Value = languageName,
					IsCurrentLanguage = currentLanguageName == languageName
				};
				mappedLanguageCollection.Add(language);
			}
			mappedLanguageModel.LanguageCollection = mappedLanguageCollection;

			return mappedLanguageModel;
		}
	}
}
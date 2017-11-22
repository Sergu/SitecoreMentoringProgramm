using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OutOfWebrotApp.Models.Components.Navigation;
using OutOfWebrotApp.Services.Interfaces.Language;
using Sitecore.Data.Managers;

namespace OutOfWebrotApp.Services.Implementations.Language
{
	public class LanguageService : ILanguageService
	{
		public Languages GetAllLanguages()
		{
			var mappedLanguageModel = new Languages();
			var mappedLanguageCollection = new List<Models.Components.Navigation.Language>();
			var sitecoreLanguageCollection = LanguageManager.GetLanguages(Sitecore.Context.Database);

			var currentLanguageName = Sitecore.Context.Item.Language.Name;

			foreach (var sitecoreLanguage in sitecoreLanguageCollection)
			{
				var languageName = sitecoreLanguage.Name;

				var language = new Models.Components.Navigation.Language()
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
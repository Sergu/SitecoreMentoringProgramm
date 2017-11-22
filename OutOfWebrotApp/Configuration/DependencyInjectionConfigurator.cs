using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using OutOfWebrotApp.Controllers.Components;
using OutOfWebrotApp.Controllers.Components.Footer;
using OutOfWebrotApp.Controllers.Pages;
using OutOfWebrotApp.Services.Implementations.Navigation;
using OutOfWebrotApp.Services.Implementations.Publishing;
using OutOfWebrotApp.Services.Implementations.Search;
using OutOfWebrotApp.Services.Interfaces.Navigation;
using OutOfWebrotApp.Services.Interfaces.Publishing;
using OutOfWebrotApp.Services.Interfaces.Search;
using System.Reflection;
using OutOfWebrotApp.Extensions;
using OutOfWebrotApp.Services.Implementations.Language;
using OutOfWebrotApp.Services.Interfaces.Language;

namespace OutOfWebrotApp.Configuration
{
	public class DependencyInjectionConfigurator : IServicesConfigurator
	{
		public void Configure(IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<INavigationService, NavigationService>();
			serviceCollection.AddSingleton<ISearchService, SearchService>();
			serviceCollection.AddSingleton<IPublishingService, PublishingService>();
			serviceCollection.AddSingleton<ISearchEngineService, SearchEngineService>();
			serviceCollection.AddSingleton<ILanguageService, LanguageService>();

			// configurator per project? Use this:
			serviceCollection.AddMvcControllersInCurrentAssembly();
		}
	}
}
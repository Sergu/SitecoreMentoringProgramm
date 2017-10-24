using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using OutOfWebrotApp.Controllers.Components;
using OutOfWebrotApp.Controllers.Components.Footer;
using OutOfWebrotApp.Services.Implementations.Navigation;
using OutOfWebrotApp.Services.Implementations.Search;
using OutOfWebrotApp.Services.Interfaces.Navigation;
using OutOfWebrotApp.Services.Interfaces.Search;

namespace OutOfWebrotApp.Configuration
{
	public class DependencyInjectionConfigurator : IServicesConfigurator
	{
		public void Configure(IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<INavigationService, NavigationService>();
			serviceCollection.AddSingleton<ISearchService, SearchService>();

			serviceCollection.AddTransient<SearchController>();
			serviceCollection.AddTransient<NavigationController>();
			serviceCollection.AddTransient<FooterController>();
		}
	}
}

using InfrastructureModule.Services.Implementations.Language;
using InfrastructureModule.Services.Implementations.Navigation;
using InfrastructureModule.Services.Implementations.Publishing;
using InfrastructureModule.Services.Implementations.Search;
using InfrastructureModule.Services.Interfaces.Language;
using InfrastructureModule.Services.Interfaces.Navigation;
using InfrastructureModule.Services.Interfaces.Publishing;
using InfrastructureModule.Services.Interfaces.Search;
using Microsoft.Extensions.DependencyInjection;
using OutOfWebrotApp.Extensions;
using Sitecore.DependencyInjection;

namespace OutOfWebrotApp.Configurator
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

			serviceCollection.AddMvcControllersInCurrentAssembly();
		}
	}
}
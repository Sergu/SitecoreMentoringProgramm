
using Glass.Mapper.Sc;
using InfrastructureModule.Services.Implementations.Email;
using InfrastructureModule.Services.Implementations.Language;
using InfrastructureModule.Services.Implementations.Navigation;
using InfrastructureModule.Services.Implementations.Publishing;
using InfrastructureModule.Services.Implementations.Search;
using InfrastructureModule.Services.Interfaces.Email;
using InfrastructureModule.Services.Interfaces.Language;
using InfrastructureModule.Services.Interfaces.Navigation;
using InfrastructureModule.Services.Interfaces.Publishing;
using InfrastructureModule.Services.Interfaces.Search;
using Microsoft.Extensions.DependencyInjection;
using OutOfWebrotApp.Extensions;
using Sitecore.Data;
using Sitecore.DependencyInjection;

namespace OutOfWebrotApp.Configurator
{
	public class DependencyInjectionConfigurator : IServicesConfigurator
	{
		public void Configure(IServiceCollection serviceCollection)
		{
			var serviceProvider = serviceCollection.BuildServiceProvider();

			serviceCollection.AddScoped<INavigationService, NavigationService>();
			serviceCollection.AddScoped<ISearchService, SearchService>();
			serviceCollection.AddScoped<IPublishingService, PublishingService>();
			serviceCollection.AddTransient<ISearchEngineService, SearchEngineService>();
			serviceCollection.AddScoped<ISearchService>(provider => new SearchService(serviceProvider.GetService<ISearchEngineService>()));
			serviceCollection.AddScoped<ILanguageService, LanguageService>();
			serviceCollection.AddScoped<IEmailService, EmailService>();

			serviceCollection.AddMvcControllersInCurrentAssembly();
		}
	}
}
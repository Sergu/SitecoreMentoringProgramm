using InfrastructureModule.Models.Components.Navigation;
using Sitecore.Data.Items;

namespace InfrastructureModule.Services.Interfaces.Navigation
{
	public interface INavigationService
	{
		NavigationModel GetNavigationModel(Item contextItem);
	}
}

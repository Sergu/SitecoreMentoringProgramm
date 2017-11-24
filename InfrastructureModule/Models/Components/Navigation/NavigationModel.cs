using System.Collections.Generic;

namespace InfrastructureModule.Models.Components.Navigation
{
	public class NavigationModel
	{
		public IEnumerable<NavigationItem> NavigationItems { get; set; }
	}
}
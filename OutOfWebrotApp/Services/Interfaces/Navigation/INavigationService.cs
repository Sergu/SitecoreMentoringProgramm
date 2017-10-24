using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OutOfWebrotApp.Models.Components.Navigation;
using Sitecore.Data.Items;

namespace OutOfWebrotApp.Services.Interfaces.Navigation
{
	public interface INavigationService
	{
		NavigationModel GetNavigationModel(Item contextItem);
	}
}

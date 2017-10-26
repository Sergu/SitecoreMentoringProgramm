using System.Web.Mvc;
using OutOfWebrotApp.Helpers;
using OutOfWebrotApp.Services.Interfaces.Navigation;

namespace OutOfWebrotApp.Controllers.Components
{
    public class NavigationController : Controller
    {
	    private readonly INavigationService _navigationService;

		public NavigationController(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}
		
        // GET: Navigation
        public ActionResult Index()
        {
	        var pageItem = SitecoreHelper.GetHomeItem();

			var navigationModel = _navigationService.GetNavigationModel(pageItem);

		    return View("~/Views/Components/Navigation/Navigation.cshtml", navigationModel);
        }
    }
}
using System.Linq;
using System.Web.Mvc;
using InfrastructureModule.Helpers;
using InfrastructureModule.Models.Components.Footer;
using InfrastructureModule.Services.Interfaces.Navigation;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;

namespace OutOfWebrotApp.Controllers.Components
{
    public class FooterController : Controller
    {
	    private readonly INavigationService _navigationService;

	    public FooterController(INavigationService navigationService)
	    {
		    _navigationService = navigationService;
	    }
        // GET: Footer
        public ActionResult Index()
        {
	        var pageItem = SitecoreHelper.GetHomeItem();

			var navigationModel = _navigationService.GetNavigationModel(pageItem);

		    return View("~/Views/Components/Footer/Footer.cshtml", navigationModel);
        }
    }
}
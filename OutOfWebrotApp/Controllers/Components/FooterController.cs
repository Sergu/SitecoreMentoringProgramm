using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;
using OutOfWebrotApp.Services.Implementations.Navigation;
using OutOfWebrotApp.Services.Interfaces.Navigation;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Extensions;

namespace OutOfWebrotApp.Controllers.Components.Footer
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
	        var pageItem = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.StartPath);

			var navigationModel = _navigationService.GetNavigationModel(pageItem);

		    return View("~/Views/Components/Footer/Footer.cshtml", navigationModel);
        }
    }
}
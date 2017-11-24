using System;
using System.Linq;
using System.Web.Mvc;
using InfrastructureModule.Helpers;
using InfrastructureModule.Services.Interfaces.Language;
using InfrastructureModule.Services.Interfaces.Navigation;

namespace OutOfWebrotApp.Controllers.Components
{
    public class NavigationController : Controller
    {
	    private readonly INavigationService _navigationService;
	    private readonly ILanguageService _languageService;

		public NavigationController(INavigationService navigationService, ILanguageService languageService)
		{
			_navigationService = navigationService;
			_languageService = languageService;
		}

        public ActionResult Index()
        {
	        var pageItem = SitecoreHelper.GetHomeItem();

			var navigationModel = _navigationService.GetNavigationModel(pageItem);

			return View("~/Views/Components/Navigation/Navigation.cshtml", navigationModel);
        }

	    public ActionResult Language()
	    {
		    var languages = _languageService.GetAllLanguages();
		    var url = this.Request.Url.AbsoluteUri;
			var uri = new Uri(url);
		    var itemUrl = uri.GetLeftPart(UriPartial.Path);
		    languages.ItemUrl = itemUrl;

		    return View("~/Views/Components/Navigation/Language.cshtml", languages);
	    }
    }
}
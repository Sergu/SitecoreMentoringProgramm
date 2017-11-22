using System;
using System.Linq;
using System.Web.Mvc;
using OutOfWebrotApp.Helpers;
using OutOfWebrotApp.Services.Interfaces.Language;
using OutOfWebrotApp.Services.Interfaces.Navigation;

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

	    [HttpGet]
	    public ActionResult Language()
	    {
		    var languages = _languageService.GetAllLanguages();

		    var url = Request.Url.AbsoluteUri;
		    var itemUrl = url.Contains("?") ? url.Split(new char[] {'?'}, StringSplitOptions.RemoveEmptyEntries).First() : url;
		    languages.ItemUrl = itemUrl;

		    return View("~/Views/Components/Navigation/Language.cshtml", languages);
	    }
    }
}
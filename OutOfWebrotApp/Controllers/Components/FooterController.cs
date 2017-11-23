using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;
using OutOfWebrotApp.Helpers;
using OutOfWebrotApp.Models.Components.Footer;
using OutOfWebrotApp.Services.Implementations.Navigation;
using OutOfWebrotApp.Services.Interfaces.Navigation;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using System.Linq;
using Sitecore.Resources.Media;

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
	        var pageItem = SitecoreHelper.GetHomeItem();

			var navigationModel = _navigationService.GetNavigationModel(pageItem);

		    return View("~/Views/Components/Footer/Footer.cshtml", navigationModel);
        }

	    public ActionResult SocialNetworks()
	    {
		    var renderingContext = RenderingContext.Current.Rendering.Item;

		    if (renderingContext == null)
		    {
			    if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
			    {
				    return View("~/Views/Errors/ExperienceEditorError.cshtml");
			    }

			    var emptySliderModel = new SocialNetworks();

			    return View("~/Views/Components/Footer/SocialNetworks.cshtml", emptySliderModel);
		    }

		    var images = renderingContext.Fields["SocialNetworks"];
		    var guidFields = new MultilistField(images);
			var guids = guidFields.GetItems();
		    var pictures = guids.Select(x => new SocialNetwork()
		    {
			    contentUrl = MediaManager.GetMediaUrl(x),
			    alt = x.Fields["Alt"].HasValue ? x.Fields["Alt"].Value : string.Empty
		    }).ToList();
		    var socialNetworksModel = new SocialNetworks()
		    {
			    SocialNetwoksIcons = pictures
		    };

			return View("~/Views/Components/Footer/SocialNetworks.cshtml", socialNetworksModel);
	    }
    }
}
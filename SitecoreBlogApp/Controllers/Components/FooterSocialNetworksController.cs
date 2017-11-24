using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InfrastructureModule.Models.Components.Footer;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;

namespace SitecoreBlogApp.Controllers.Components
{
    public class FooterSocialNetworksController : Controller
    {
        // GET: FooterSocialNetworks
        public ActionResult Index()
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
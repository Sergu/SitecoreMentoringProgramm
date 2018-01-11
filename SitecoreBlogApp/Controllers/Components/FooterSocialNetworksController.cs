using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Sc.Web.Mvc;
using InfrastructureModule.Helpers;
using InfrastructureModule.Models.Components.Footer;
using InfrastructureModule.TDS.sitecore.templates.Custom.SitecoreBlogTemplates.Components.Footer;
using InfrastructureModule.TDS.sitecore.templates.System.Media.Unversioned;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;

namespace SitecoreBlogApp.Controllers.Components
{
    public class FooterSocialNetworksController : GlassController
    {
        // GET: FooterSocialNetworks
        public override ActionResult Index()
        {
	        var sitecoreService = this.SitecoreContext;
			var renderingContext = GetDataSourceItem<IFooterDataSource>();

	        if (renderingContext == null)
	        {
		        if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
		        {
			        return View("~/Views/Errors/ExperienceEditorError.cshtml");
		        }

		        var emptySliderModel = new SocialNetworks();

		        return View("~/Views/Components/Footer/SocialNetworks.cshtml", emptySliderModel);
	        }

	        var images = renderingContext.SocialNetworks.Select(id => sitecoreService.GetItem<Item>(id)).ToList();
	        var pictures = images.Select(x => new SocialNetwork()
	        {
		        contentUrl = MediaManager.GetMediaUrl(x),
		        alt = sitecoreService.GetItem<IImage>(x.ID.Guid).Alt
	        }).ToList();
	        var socialNetworksModel = new SocialNetworks()
	        {
		        SocialNetwoksIcons = pictures
	        };

	        return View("~/Views/Components/Footer/SocialNetworks.cshtml", socialNetworksModel);
		}
    }
}
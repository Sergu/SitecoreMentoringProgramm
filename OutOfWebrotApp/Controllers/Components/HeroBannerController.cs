using System;
using System.Linq;
using System.Runtime.InteropServices;
using InfrastructureModule.Helpers;
using System.Web.Mvc;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using InfrastructureModule.Models.Components.HeroBanner;
using InfrastructureModule.Models.Components.Slider;
using InfrastructureModule.TDS.sitecore.templates.Custom.BaseTemplates.Components.Slider.Base;
using InfrastructureModule.TDS.sitecore.templates.System.Media.Unversioned;
using Sitecore.Data.Fields;
using Sitecore.Resources.Media;
using Sitecore.Data.Items;

namespace OutOfWebrotApp.Controllers.Components
{
    public class HeroBannerController : GlassController
    {
        // GET: HeroBanner
        public override ActionResult Index()
        {
	        var contextService = this.SitecoreContext;
	        var siteSettingsItem = SitecoreHelper.GetSiteSettingItem(contextService);
	        if (siteSettingsItem.SliderDataSource == Guid.Empty)
	        {
		        throw new ArgumentException("siteSettingsItem.SliderDataSource id is empty");
	        }

	        var sliderDataSourceItem = contextService.GetItem<ISliderBase>(siteSettingsItem.SliderDataSource);

	        if (sliderDataSourceItem == null)
	        {
		        throw new NullReferenceException("sliderDataSourceItem is null");
	        }

	        var images = sliderDataSourceItem.Images.Select(i => contextService.GetItem<Item>(i)).ToList();
	        var picture = images.Select(x => new Picture()
	        {
		        contentUrl = MediaManager.GetMediaUrl(x),
		        alt = contextService.GetItem<IImage>(x.ID.Guid).Alt
	        }).ToList().FirstOrDefault();
	        var heroBannerModel = new HeroBanner()
	        {
		        Picture = picture
	        };

			return View("~/Views/Components/HeroBanner/HeroBanner.cshtml", heroBannerModel);
        }
    }
}
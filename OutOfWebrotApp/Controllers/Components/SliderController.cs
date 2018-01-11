using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using InfrastructureModule.Helpers;
using InfrastructureModule.Models.Components.Slider;
using InfrastructureModule.TDS.sitecore.templates.Custom.BaseTemplates.Components.Slider.Base;
using InfrastructureModule.TDS.sitecore.templates.Custom.BaseTemplates.Components.Slider.Rendering_parametres;
using InfrastructureModule.TDS.sitecore.templates.Custom.BaseTemplates.Components.Slider.Settings;
using InfrastructureModule.TDS.sitecore.templates.Custom.BaseTemplates.Settings;
using InfrastructureModule.TDS.sitecore.templates.System.Media.Unversioned;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;

namespace OutOfWebrotApp.Controllers.Components
{
    public class SliderController : GlassController
    {
        // GET: Slider
        public override ActionResult Index()
        {
	        int sliderSpeed;

	        var contextService = this.SitecoreContext;
	        var siteSettingsItem = SitecoreHelper.GetSiteSettingItem(contextService);
	        var sliderDataSourceItem = contextService.GetItem<ISliderBase>(siteSettingsItem.SliderDataSource);
	        if (sliderDataSourceItem == null)
	        {
		        if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
		        {
			        return View("~/Views/Errors/ExperienceEditorError.cshtml");
		        }

		        var emptySliderModel = new Slider()
		        {
			        Pictures = new List<Picture>()
		        };

		        return View("~/Views/Components/Slider/Slider.cshtml", emptySliderModel);
			}

	        var sliderParameters = GetRenderingParameters<ISliderParameterTemplate>();
			var defaultSpeedItem = contextService.GetItem<ISliderSpeedSetting>(siteSettingsItem.SliderDefaultSpeedTemplatePath);
	        var defaultSliderSpeed = 3000;
			if (!int.TryParse(sliderParameters.Speed, out sliderSpeed))
	        {
		        sliderSpeed = defaultSliderSpeed;
	        }

	        var images = sliderDataSourceItem.Images.Select(i => contextService.GetItem<Item>(i)).ToList();
	        var pictures = images.Select(x => new Picture()
	        {
		        contentUrl = MediaManager.GetMediaUrl(x),
				alt = contextService.GetItem<IImage>(x.ID.Guid).Alt
			}).ToList();
	        var sliderModel = new Slider()
	        {
		        Pictures = pictures,
				Speed	= sliderSpeed
	        };

	        return View("~/Views/Components/Slider/Slider.cshtml", sliderModel);
        }
    }
}
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using InfrastructureModule.Helpers;
using InfrastructureModule.Models.Components.Slider;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;

namespace OutOfWebrotApp.Controllers.Components
{
    public class SliderController : Controller
    {
        // GET: Slider
        public ActionResult Index()
        {
	        int sliderSpeed;
	        //var renderingContext = RenderingContext.Current.Rendering.Item;

	        var siteSettingsItem = SitecoreHelper.GetSiteSettingItem();
	        var sliderDataSourceItemId = siteSettingsItem.Fields["SliderDataSource"].Value;
	        var sliderDataSourceItem = Sitecore.Context.Database.GetItem(sliderDataSourceItemId);

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

			var sliderSpeedRenderingParameter = RenderingContext.Current.Rendering.Parameters.Contains("Speed")
		        ? RenderingContext.Current.Rendering.Parameters["Speed"]
		        : null;

	        var defaultSliderSpeedTemplatePath = SitecoreHelper.GetSiteSettingItem().Fields["SliderDefaultSpeedTemplatePath"].Value;
	        var defaultSpeedItem = Sitecore.Context.Database.GetItem(defaultSliderSpeedTemplatePath);

			//var defaultSliderSpeed = int.Parse(defaultSpeedItem.Fields["Speed"].Value);
	        var defaultSliderSpeed = 3000;
			if (!int.TryParse(sliderSpeedRenderingParameter, out sliderSpeed))
	        {
		        sliderSpeed = defaultSliderSpeed;
	        }
			var images = sliderDataSourceItem.Fields["Images"];
	        var guidFields = new MultilistField(images);
	        var guids = guidFields.GetItems();
	        var pictures = guids.Select(x => new Picture()
	        {
		        contentUrl = MediaManager.GetMediaUrl(x),
				alt = x.Fields["Alt"].HasValue ? x.Fields["Alt"].Value : string.Empty
	        }).ToList();
	        var sliderModel = new Slider()
	        {
		        Pictures = pictures,
				Speed	= sliderSpeed,
				ContextItem = sliderDataSourceItem
	        };

	        return View("~/Views/Components/Slider/Slider.cshtml", sliderModel);
        }
    }
}
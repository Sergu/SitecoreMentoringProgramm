using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using OutOfWebrotApp.Models.Components.Slider;
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
	        var renderingContext = RenderingContext.Current.Rendering.Item;

	        if (renderingContext == null)
	        {
		        if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
		        {
			        return View("~/Views/Errors/ExperienceEditorError.cshtml");
		        }

		        var emptySliderModel = new Models.Components.Slider.Slider()
		        {
			        Pictures = new List<Picture>()
		        };

		        return View("~/Views/Components/Slider/Slider.cshtml", emptySliderModel);
			}

			var sliderSpeedRenderingParameter = RenderingContext.Current.Rendering.Parameters.Contains("Speed")
		        ? RenderingContext.Current.Rendering.Parameters["Speed"]
		        : null;

	        var defaultSliderSpeedTemplatePath = ConfigurationManager.AppSettings["SliderDefaultSpeedTemplatePath"];
	        var defaultSliderSpeed = int.Parse(Sitecore.Context.Database.GetItem(defaultSliderSpeedTemplatePath).Fields["Speed"].Value);
	        if (!int.TryParse(sliderSpeedRenderingParameter, out sliderSpeed))
	        {
		        sliderSpeed = defaultSliderSpeed;
	        }
			var images = renderingContext.Fields["Images"];
	        var guidFields = new MultilistField(images);
	        var guids = guidFields.GetItems();
	        var pictures = guids.Select(x => new Picture()
	        {
		        contentUrl = MediaManager.GetMediaUrl(x),
				alt = x.Fields["Alt"].HasValue ? x.Fields["Alt"].Value : string.Empty
	        }).ToList();
	        var sliderModel = new Models.Components.Slider.Slider()
	        {
		        Pictures = pictures,
				Speed	= sliderSpeed
	        };

	        return View("~/Views/Components/Slider/Slider.cshtml", sliderModel);
        }
    }
}
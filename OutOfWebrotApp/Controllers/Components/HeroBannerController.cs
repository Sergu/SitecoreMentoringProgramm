using System.Linq;
using System.Runtime.InteropServices;
using InfrastructureModule.Helpers;
using System.Web.Mvc;
using InfrastructureModule.Models.Components.HeroBanner;
using InfrastructureModule.Models.Components.Slider;
using Sitecore.Data.Fields;
using Sitecore.Resources.Media;

namespace OutOfWebrotApp.Controllers.Components
{
    public class HeroBannerController : Controller
    {
        // GET: HeroBanner
        public ActionResult Index()
        {
	        var siteSettingsItem = SitecoreHelper.GetSiteSettingItem();
	        var sliderDataSourceItemId = siteSettingsItem.Fields["SliderDataSource"].Value;
	        var sliderDataSourceItem = Sitecore.Context.Database.GetItem(sliderDataSourceItemId);

	        var images = sliderDataSourceItem.Fields["Images"];
	        var guidFields = new MultilistField(images);
	        var guids = guidFields.GetItems();
	        var picture = guids.Select(x => new Picture()
	        {
		        contentUrl = MediaManager.GetMediaUrl(x),
		        alt = x.Fields["Alt"].HasValue ? x.Fields["Alt"].Value : string.Empty
	        }).ToList().FirstOrDefault();
	        var heroBannerModel = new HeroBanner()
	        {
		        Picture = picture
	        };

			return View("~/Views/Components/HeroBanner/HeroBanner.cshtml", heroBannerModel);
        }
    }
}
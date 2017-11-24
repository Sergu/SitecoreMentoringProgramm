using System.Web.Mvc;
using InfrastructureModule.Models.Pages.About;
using Sitecore.Mvc.Presentation;

namespace OutOfWebrotApp.Controllers.Pages
{
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult Index()
        {
			var context = RenderingContext.Current.Rendering.Item;

	        if (context == null)
	        {
		        if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
		        {
			        return View("~/Views/Errors/ExperienceEditorError.cshtml");
		        }

				return View("~/Views/Pages/About/About.cshtml", new AboutPageModel());
			}

			var aboutModel = new AboutPageModel()
	        {
		        Body = context.Fields["Body"].Value,
			};
			

			return View("~/Views/Pages/About/About.cshtml", aboutModel);
        }
    }
}
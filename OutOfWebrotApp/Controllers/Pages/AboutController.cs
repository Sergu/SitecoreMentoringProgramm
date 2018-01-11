using System.Web.Mvc;
using Glass.Mapper.Sc.Web.Mvc;
using InfrastructureModule.Models.Pages.About;
using InfrastructureModule.TDS.sitecore.templates.Custom.BaseTemplates.Pages.About;
using Sitecore.Mvc.Presentation;

namespace OutOfWebrotApp.Controllers.Pages
{
    public class AboutController : GlassController
    {
        // GET: About
        public override ActionResult Index()
        {
	        var sitecoreService = this.SitecoreContext;
			var contextAboutItem = GetContextItem<IAbout>();

	        if (contextAboutItem == null)
	        {
		        if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
		        {
			        return View("~/Views/Errors/ExperienceEditorError.cshtml");
		        }

				return View("~/Views/Pages/About/About.cshtml", new AboutPageModel());
			}

			var aboutModel = new AboutPageModel()
	        {
		        Body = contextAboutItem.Body,
			};
			

			return View("~/Views/Pages/About/About.cshtml", aboutModel);
        }
    }
}
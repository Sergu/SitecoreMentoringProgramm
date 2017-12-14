using System.Web.Mvc;
using InfrastructureModule.Models.Pages.Errors;
using Sitecore.Mvc.Presentation;

namespace BlogApp.Controllers.Pages
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult PageNotFound()
        {
	        var context = RenderingContext.Current.ContextItem;
	        var model = new PageNotFound()
	        {
		        Title = context.Fields["Title"].Value,
		        Content = context.Fields["ContentField"].Value
	        };

	        return View("~/Views/Pages/Errors/PageNotFound.cshtml", model);
        }
    }
}
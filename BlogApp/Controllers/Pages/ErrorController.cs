using System;
using System.Web.Mvc;
using Glass.Mapper.Sc.Web.Mvc;
using InfrastructureModule.Helpers;
using InfrastructureModule.Models.Pages.Errors;
using InfrastructureModule.TDS.sitecore.templates.Custom.BlogApp.Pages.NotFound;
using Sitecore.Mvc.Presentation;

namespace BlogApp.Controllers.Pages
{
    public class ErrorController : GlassController
    {
        // GET: Error
        public ActionResult PageNotFound()
        {
	        var sitecoreService = this.SitecoreContext;
	        var pageNotFoundTdsItem = SitecoreHelper.GetNotFoundPageTdsItem(sitecoreService);
			if(pageNotFoundTdsItem == null)
				throw new NullReferenceException("pageNotFoundTdsItem is null"); 
			
	        var model = new PageNotFound()
	        {
		        Title = pageNotFoundTdsItem.Title,
		        Content = pageNotFoundTdsItem.ContentField
	        };

	        return View("~/Views/Pages/Errors/PageNotFound.cshtml", model);
        }
    }
}
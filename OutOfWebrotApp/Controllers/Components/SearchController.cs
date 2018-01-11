using System.Web.Mvc;
using Glass.Mapper.Sc.Web.Mvc;
using InfrastructureModule.Helpers;
using InfrastructureModule.Models.Pages.Search;
using InfrastructureModule.Services.Interfaces.Search;
using Sitecore.Links;

namespace OutOfWebrotApp.Controllers.Components
{
    public class SearchController : GlassController
    {
	    private readonly ISearchService _searchService;

	    public SearchController(ISearchService searchService)
	    {
		    _searchService = searchService;
	    }
        // GET: Search
        public ActionResult SearchHeaderComponent()
        {
            return View("~/Views/Components/Navigation/Search.cshtml");
        }

	    [HttpPost]
	    public ActionResult SearchHeaderComponent(SearchModel searchModel)
	    {
		    var searchItem = SitecoreHelper.GetPostsItem();
		    var searchItemUrl = LinkManager.GetItemUrl(searchItem);

		    return Redirect($"{searchItemUrl}?s={searchModel.Title}");
	    }
    }
}
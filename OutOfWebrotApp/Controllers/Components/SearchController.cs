using System.Linq;
using System.Web;
using System.Web.Mvc;
using OutOfWebrotApp.Helpers;
using OutOfWebrotApp.Models.Pages.Search;
using OutOfWebrotApp.Services.Interfaces.Search;
using Sitecore.Links;

namespace OutOfWebrotApp.Controllers.Components
{
    public class SearchController : Controller
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
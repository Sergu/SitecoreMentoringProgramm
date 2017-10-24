using System.Linq;
using System.Web;
using System.Web.Mvc;
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
		    var startItemPath = Sitecore.Context.Site.StartPath;
		    var searchItemQuery = $"{startItemPath}//*[@@templatekey='search']";
		    var searchItem = Sitecore.Context.Database.SelectItems(searchItemQuery);
		    var searchItemUrl = LinkManager.GetItemUrl(searchItem.First());

		    return Redirect($"{searchItemUrl}?s={searchModel.Title}");
	    }

	    public ActionResult SearchResult()
	    {
		    var url = Request.Url;
		    string param = HttpUtility.ParseQueryString(url.Query).Get("s");
		    var searchResult = _searchService.SearchPostsByTitle(param);

		    return View("~/Views/Pages/Search/SearchResult.cshtml", searchResult);
	    }
    }
}
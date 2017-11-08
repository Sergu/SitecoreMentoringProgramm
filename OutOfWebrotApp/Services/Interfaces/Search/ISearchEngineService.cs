using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OutOfWebrotApp.Models.Components.SearchIndex;
using OutOfWebrotApp.Models.Services.SearchEngineService;
using Sitecore.Data;
using Sitecore.Search;
using Sitecore.ContentSearch.Linq;

namespace OutOfWebrotApp.Services.Interfaces.Search
{
	public interface ISearchEngineService
	{
		SearchEngineSearchResult SearchPosts(string title, int page, int pageCapacity, IList<ID> tags, IList<ID> categories);
	}
}
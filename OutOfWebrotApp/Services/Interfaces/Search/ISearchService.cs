using System.Collections.Generic;
using OutOfWebrotApp.Models.Pages.Search;
using Sitecore.Data;

namespace OutOfWebrotApp.Services.Interfaces.Search
{
	public interface ISearchService
	{
		SearchModel SearchPostsByTitle(string title);
		SearchModel GetPosts(string title, int page, IList<ID> tags, IList<ID> categories);
	}
}
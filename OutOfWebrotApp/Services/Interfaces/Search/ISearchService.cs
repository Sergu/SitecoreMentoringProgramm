using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OutOfWebrotApp.Models.Pages.Posts;
using OutOfWebrotApp.Models.Pages.Search;

namespace OutOfWebrotApp.Services.Interfaces.Search
{
	public interface ISearchService
	{
		SearchModel SearchPostsByTitle(string title);
	}
}
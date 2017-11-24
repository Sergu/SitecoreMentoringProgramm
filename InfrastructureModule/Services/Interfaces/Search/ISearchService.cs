using System.Collections.Generic;
using InfrastructureModule.Models.Pages.Search;
using Sitecore.Data;

namespace InfrastructureModule.Services.Interfaces.Search
{
	public interface ISearchService
	{
		SearchModel SearchPostsByTitle(string title);
		SearchModel SearchPosts(string title, int page, IList<ID> tags, IList<ID> categories);
	}
}
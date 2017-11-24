using System.Collections.Generic;
using InfrastructureModule.Models.Services.SearchEngineService;
using Sitecore.Data;

namespace InfrastructureModule.Services.Interfaces.Search
{
	public interface ISearchEngineService
	{
		SearchEngineSearchResult SearchPosts(string title, int page, int pageCapacity, IList<ID> tags, IList<ID> categories);
	}
}
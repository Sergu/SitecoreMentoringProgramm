using System;
using System.Collections.Generic;
using InfrastructureModule.Models.Components.SearchIndex;
using InfrastructureModule.Models.Services.SearchEngineService;
using Sitecore.ContentSearch.Linq;
using Sitecore.Data;

namespace InfrastructureModule.Services.Interfaces.Search
{
	public interface ISearchEngineService
	{
		SearchEngineSearchResult SearchPosts(string indexName, string title, int page, int pageCapacity, IList<ID> tags, IList<ID> categories);

		List<SearchHit<PostSearchIndexModel>> SearchPosts(string indexName, DateTime date);
	}
}
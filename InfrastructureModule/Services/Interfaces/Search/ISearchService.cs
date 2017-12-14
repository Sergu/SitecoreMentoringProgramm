using System.Collections.Generic;
using InfrastructureModule.Models.Pages.Search;
using Sitecore.Data;
using System;
using InfrastructureModule.Models.Emailing;

namespace InfrastructureModule.Services.Interfaces.Search
{
	public interface ISearchService
	{
		SearchModel SearchPostsByTitle(string title);
		SearchModel SearchPosts(string indexName, string title, int page, IList<ID> tags, IList<ID> categories);
		FreshPostsResult GetFreshPostsSinceDate(string indexName, DateTime date);
	}
}
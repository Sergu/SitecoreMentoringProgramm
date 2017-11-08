using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OutOfWebrotApp.Models.Components.SearchIndex;
using Sitecore.ContentSearch.Linq;

namespace OutOfWebrotApp.Models.Services.SearchEngineService
{
	public class SearchEngineSearchResult
	{
		public int PageCapacity { get; set; }
		public int CurrentPage { get; set; }
		public int TotalAmount { get; set; }
		public List<SearchHit<PostSearchIndexModel>> SearchResut { get; set; }
	}
}
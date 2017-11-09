using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OutOfWebrotApp.Models.Pages.Post;

namespace OutOfWebrotApp.Models.Pages.Search
{
	public class SearchModel
	{
		public SearchModel()
		{
			SearchResult = new List<PostItemModel>();
			Tags = new List<string>();
			Categories = new List<string>();
		}

		public string Title { get; set; }
		public IEnumerable<string> Tags { get; set; }
		public IEnumerable<string> Categories { get; set; }
		public int Page { get; set; }
		public int TotalPageAmount { get; set; }
		public int PageCapacity { get; set; }
		public int TotalPostAmount { get; set; }
		public IList<PostItemModel> SearchResult { get; set; }
		public bool IsSuccessful { get; set; }
		public string NoResultMessage { get; set; }
	}
}
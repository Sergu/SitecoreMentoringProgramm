using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutOfWebrotApp.Models.Pages.Search
{
	public class SearchModel
	{
		public string Title { get; set; }
		public Posts.Posts SearchResult { get; set; }
		public bool IsSuccessful { get; set; }
	}
}
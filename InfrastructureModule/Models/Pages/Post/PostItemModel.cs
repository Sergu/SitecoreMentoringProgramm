using System;
using System.Collections.Generic;

namespace InfrastructureModule.Models.Pages.Post
{
	public class PostItemModel
	{
		public PostItemModel()
		{
			Tags = new List<string>();
		}

		public string Title { get; set; }
		public string Body { get; set; }
		public string Subtitle { get; set; }
		public string Url { get; set; }
		public string Author { get; set; }
		public DateTime Date { get; set; }
		public string Category { get; set; }
		public IEnumerable<string> Tags { get; set; }
	}
}
using System;
using System.Collections.Generic;

namespace OutOfWebrotApp.Models.Pages.Post
{
	public class PostItemModel
	{
		public string Title { get; set; }
		public string Body { get; set; }
		public string Subtitle { get; set; }
		public string Url { get; set; }
		public string Author { get; set; }
		public DateTime Date { get; set; }
	}
}
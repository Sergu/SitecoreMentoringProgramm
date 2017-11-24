using System;

namespace InfrastructureModule.Models.Pages.Post
{
	public class Comment
	{
		public string Author { get; set; }
		public DateTime Date { get; set; }
		public string Text { get; set; }
	}
}
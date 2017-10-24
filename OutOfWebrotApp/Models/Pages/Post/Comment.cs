using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutOfWebrotApp.Models.Pages.Post
{
	public class Comment
	{
		public string Author { get; set; }
		public DateTime Date { get; set; }
		public string Text { get; set; }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OutOfWebrotApp.Models.Pages.Post
{
	public class Comment
	{
		public string Author { get; set; }
		public DateTime Date { get; set; }
		[AllowHtml]
		public string Text { get; set; }
	}
}
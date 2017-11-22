using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutOfWebrotApp.Models.Components.Navigation
{
	public class Languages
	{
		public Languages()
		{
			LanguageCollection = new List<Language>();
		}

		public string ItemUrl { get; set; }
		public IList<Language> LanguageCollection { get; set; }
	}
}
using System.Collections.Generic;

namespace InfrastructureModule.Models.Components.Navigation
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
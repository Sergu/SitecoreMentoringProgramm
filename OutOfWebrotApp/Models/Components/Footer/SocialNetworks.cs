using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutOfWebrotApp.Models.Components.Footer
{
	public class SocialNetworks
	{
		public SocialNetworks()
		{
			SocialNetwoksIcons = new List<SocialNetwork>();
		}
		public IList<SocialNetwork> SocialNetwoksIcons { get; set; }
	}
}
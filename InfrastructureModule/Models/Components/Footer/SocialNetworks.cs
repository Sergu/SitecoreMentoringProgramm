using System.Collections.Generic;

namespace InfrastructureModule.Models.Components.Footer
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
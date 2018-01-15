using System;
using System.Collections.Generic;
using Sitecore.Data.Items;

namespace InfrastructureModule.Models.Components.Slider
{
	public class Slider
	{
		public int Speed { get; set; }
		public IList<Picture> Pictures { get; set; }
		public Guid ContextItemId { get; set; }
	}
}
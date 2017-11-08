using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Sitecore.Data.Converters;

namespace OutOfWebrotApp.Models.Components.TagsTree.Serialization
{
	public class Subtree
	{
		[JsonProperty("id")]
		public string id;
		[JsonProperty("text")]
		public string text;
		[JsonProperty("item")]
		public IEnumerable<Subtree> children;
	}
}
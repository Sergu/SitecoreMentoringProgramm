using System.Collections.Generic;
using Newtonsoft.Json;

namespace InfrastructureModule.Models.Components.TagsTree.Serialization
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
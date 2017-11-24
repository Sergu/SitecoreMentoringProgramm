using System.Collections.Generic;
using InfrastructureModule.Models.Components.TagsTree.Serialization;
using Sitecore.Data.Items;

namespace InfrastructureModule.Helpers
{
	public class TagsTreeHelper
	{

		public static JsTreeData GetTagsTree(Item rootItem)
		{
			var tree = new JsTreeData()
			{
				data = GetSubtreeJson(rootItem)
			};

			return tree;
		}

		private static IEnumerable<Subtree> GetSubtreeJson(Item subTree)
		{
			var result = new List<Subtree>();
			var childItems = subTree.Children;

			if (childItems == null)
			{
				return null;
			}

			foreach (Item childItem in childItems)
			{
				var item = new Subtree()
				{
					id = childItem.ID.ToString(),
					text = childItem.Fields["Value"].Value,
					children = GetSubtreeJson(childItem),
				};

				result.Add(item);
			}

			return result;
		}
	}
}
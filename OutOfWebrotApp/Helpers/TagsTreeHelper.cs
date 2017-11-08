using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Helpers;
using OutOfWebrotApp.Models.Components.TagsTree.Serialization;
using Sitecore.Data.Items;

namespace OutOfWebrotApp.Helpers
{
	public class TagsTreeHelper
	{

		public static string GetTagsTreeJson(Item rootItem)
		{
			var tree = new JsTreeData()
			{
				data = GetSubtreeJson(rootItem)
			};

			return Json.Encode(tree);
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
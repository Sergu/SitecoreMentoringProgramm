using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.StringExtensions;

namespace OutOfWebrotApp.Search.ComputedFields
{
	public class PostTagsStringComputedField : IComputedIndexField
	{
		public string FieldName { get; set; }
		public string ReturnType { get; set; }

		public object ComputeFieldValue(IIndexable indexable)
		{
			var item = (SitecoreIndexableItem)indexable;
			if (indexable == null)
			{
				return string.Empty;
			}

			MultilistField itemDataField = ((SitecoreItemDataField) item.GetFieldByName("Tags")).Field;

			var result = string.Empty;
			var tagItems = itemDataField.GetItems();

			foreach (var tag in tagItems)
			{
				var title = tag.Fields["Value"].Value;
				if (title.IsNullOrEmpty())
				{
					title = tag.Name;
				}

				result += title + " ";
			}

			return result.TrimEnd(' ');
		}
	}
}
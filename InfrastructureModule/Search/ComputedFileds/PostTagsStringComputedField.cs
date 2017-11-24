using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Managers;
using Sitecore.StringExtensions;

namespace InfrastructureModule.Search.ComputedFileds
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

			var languageCollection = LanguageManager.GetLanguages(Sitecore.Context.Database);

			var langs = languageCollection.Where(l => l.Name == item.Culture.Name);
			var currentLanguage = langs.FirstOrDefault();

			MultilistField itemDataField = ((SitecoreItemDataField) item.GetFieldByName("Tags")).Field;

			var result = string.Empty;
			var tagItems = itemDataField.GetItems();

			foreach (var tag in tagItems)
			{
				var culturalTagItem = currentLanguage != null ? tag.Database.GetItem(tag.ID, currentLanguage) : null;

				var title = culturalTagItem.Fields["Value"].Value;
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
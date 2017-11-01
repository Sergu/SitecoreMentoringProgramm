using Sitecore.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Sitecore.ContentSearch.Converters;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Fields;
using Sitecore.ContentSearch;

namespace OutOfWebrotApp.Models.Components.SearchIndex
{
	public class PostSearchIndexModel : SearchResultItem
	{
		[DataMember]
		[IndexField("Title")]
		public virtual string Title { get; set; }
		[DataMember]
		[IndexField("Subtitle")]
		public virtual string Subtitle { get; set; }
	}
}
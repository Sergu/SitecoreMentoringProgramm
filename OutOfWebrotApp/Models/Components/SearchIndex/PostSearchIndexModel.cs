using Sitecore.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Sitecore.ContentSearch.Converters;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Fields;
using Sitecore.ContentSearch;
using Sitecore.Data;

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
		[DataMember]
		[IndexField("Category")]
		[TypeConverter(typeof(IndexFieldIDValueConverter))]
		public virtual ID Category { get; set; }
		[DataMember]
		[IndexField("post_tags_string")]
		public virtual string PostTagsString { get; set; }
		[DataMember]
		[IndexField("Tags")]
		[TypeConverter(typeof(IndexFieldEnumerableConverter))]
		public virtual IEnumerable<ID> Tags { get; set; }
	}
}
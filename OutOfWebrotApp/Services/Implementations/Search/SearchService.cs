using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using OutOfWebrotApp.Helpers;
using OutOfWebrotApp.Models.Components.SearchIndex;
using OutOfWebrotApp.Models.Pages.Post;
using OutOfWebrotApp.Models.Pages.Posts;
using OutOfWebrotApp.Models.Pages.Search;
using OutOfWebrotApp.Services.Interfaces.Search;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data.Fields;
using Sitecore.Links;

namespace OutOfWebrotApp.Services.Implementations.Search
{
	public class SearchService : ISearchService
	{
		public int GetSearchResultNumber(string title)
		{
			var index = ContentSearchManager.GetIndex("post_web_index");
			using (var context = index.CreateSearchContext())
			{
				//var predicateBuilder = PredicateBuilder.True<PostSearchIndexModel>();
				//predicateBuilder.And(c => c.Title.Length == 0);
				//var results = context.GetQueryable<PostSearchIndexModel>()
				//	.Where(predicateBuilder)
				//	.GetResults();

				var results = context.GetQueryable<PostSearchIndexModel>()
					.Where(c => c.Title.Length > 0)
					.Page(0,5)
					.GetResults();
				var s = results.ToList();
				return s.Count;
			}
		}

		public SearchModel SearchPostsByTitle(string title)
		{
			var searchModel = new SearchModel();
			searchModel.Title = title;
			var contextItem = SitecoreHelper.GetPostsItem();
			var searchPostsQuerry = $"{contextItem.Paths.FullPath}/*[contains(@title,'{title}') and @@templatekey='post']";

			var searchResultItems = Sitecore.Context.Database.SelectItems(searchPostsQuerry);

			if (searchResultItems != null && searchResultItems.Length > 0)
			{
				var postCollection = new List<PostItemModel>();

				foreach (var item in searchResultItems)
				{
					var dateField = item.Fields["Date"].HasValue ? new DateField(item.Fields["Date"]).DateTime : DateTime.Now;


					var post = new PostItemModel()
					{
						Body = item.Fields["Body"].HasValue ? item.Fields["Body"].Value : string.Empty,
						Subtitle = item.Fields["Subtitle"].HasValue ? item.Fields["Subtitle"].Value : string.Empty,
						Title = item.Fields["Title"].HasValue ? item.Fields["Title"].Value : string.Empty,
						Url = LinkManager.GetItemUrl(item),
						Author = item.Fields["Author"].HasValue ? item.Fields["Author"].Value : string.Empty,
						Date = dateField
					};
					postCollection.Add(post);
				}
				searchModel.IsSuccessful = true;
				searchModel.SearchResult = new Posts() {PostsCollection = postCollection};
			}
			else
			{
				searchModel.IsSuccessful = false;
			}

			return searchModel;
		}
	}
}
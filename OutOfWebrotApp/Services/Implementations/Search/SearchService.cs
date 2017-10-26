using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using OutOfWebrotApp.Helpers;
using OutOfWebrotApp.Models.Pages.Post;
using OutOfWebrotApp.Models.Pages.Posts;
using OutOfWebrotApp.Models.Pages.Search;
using OutOfWebrotApp.Services.Interfaces.Search;
using Sitecore.Data.Fields;
using Sitecore.Links;

namespace OutOfWebrotApp.Services.Implementations.Search
{
	public class SearchService : ISearchService
	{
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
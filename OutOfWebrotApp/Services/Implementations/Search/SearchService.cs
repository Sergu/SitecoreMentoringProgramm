using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using OutOfWebrotApp.Helpers;
using OutOfWebrotApp.Models.Pages.Post;
using OutOfWebrotApp.Models.Pages.Search;
using OutOfWebrotApp.Services.Interfaces.Search;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Links;
using Sitecore.Mvc.Extensions;

namespace OutOfWebrotApp.Services.Implementations.Search
{
	public class SearchService : ISearchService
	{
		private readonly int _postsCapacity;
		private readonly ISearchEngineService _searchEngineService;

		public SearchService(ISearchEngineService searchEngineService)
		{
			var item = SitecoreHelper.GetSiteSettingItem();
			var capacityConfigValue = item.Fields["PostsPageCapacity"].Value;
			int postsCapacity;
			if (capacityConfigValue.IsEmptyOrNull() || !int.TryParse(capacityConfigValue, out postsCapacity))
			{
				throw new ArgumentException("Incorrect PostsPageCapacity parameter");
			}

			_searchEngineService = searchEngineService;
			_postsCapacity = postsCapacity;
		}

		public SearchModel SearchPosts(string title, int page, IList<ID> tags, IList<ID> categories)
		{
			var searchResult = _searchEngineService.SearchPosts(title, page, _postsCapacity, tags, categories);
			var siteSettingItem = SitecoreHelper.GetSiteSettingItem();

			if (searchResult.SearchResut.Count() == 0)
			{
				return new SearchModel()
				{
					IsSuccessful = false,
					NoResultMessage = siteSettingItem.Fields["NoResultMessage"].Value
				};
			}

			var postCollection = new List<PostItemModel>();

			foreach (var result in searchResult.SearchResut)
			{
				var resultItem = result.Document.GetItem();
				MultilistField postTagsField = resultItem.Fields["Tags"];
				LookupField category = resultItem.Fields["Category"];

				var post = new PostItemModel()
				{
					Body = resultItem.Fields["Body"].Value,
					Subtitle = resultItem.Fields["Subtitle"].Value,
					Title = resultItem.Fields["Title"].Value,
					Url = LinkManager.GetItemUrl(resultItem),
					Author = resultItem.Fields["Author"].Value,
					Date = (new DateField(resultItem.Fields["Date"])).DateTime,
					Tags = postTagsField.GetItems().Select(i => i.Fields["Value"].Value),
					Category = category.TargetItem.Fields["Value"].Value,
				};

				postCollection.Add(post);
			}

			return new SearchModel()
			{
				SearchResult = postCollection.OrderByDescending(m => m.Date).ToList(),
				IsSuccessful = true,
				Page = searchResult.CurrentPage,
				TotalPageAmount = searchResult.TotalPageAmount,
				PageCapacity = searchResult.PageCapacity,
				TotalPostAmount = searchResult.TotalPostAmount
			};

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
				searchModel.SearchResult = postCollection;
			}
			else
			{
				searchModel.IsSuccessful = false;
			}

			return searchModel;
		}
	}
}
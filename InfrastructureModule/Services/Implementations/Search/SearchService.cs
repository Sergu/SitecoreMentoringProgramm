using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
using InfrastructureModule.Helpers;
using InfrastructureModule.Models.Pages.Post;
using InfrastructureModule.Models.Pages.Search;
using InfrastructureModule.Services.Interfaces.Search;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Links;
using Sitecore.StringExtensions;
using InfrastructureModule.Models.Components.SearchIndex;
using InfrastructureModule.Models.Emailing;
using Sitecore.ContentSearch.Linq;

namespace InfrastructureModule.Services.Implementations.Search
{
	public class SearchService : ISearchService
	{
		private readonly int _postsCapacity;
		private readonly ISearchEngineService _searchEngineService;
		private readonly ISitecoreService _sitecoreService;

		public SearchService(ISearchEngineService searchEngineService)
		{
			searchEngineService = new SearchEngineService();
			var sitecoreService = new SitecoreService(Sitecore.Context.Database);
			
			if(searchEngineService == null || sitecoreService == null)
				throw new NullReferenceException("sitecoreService or searchEngineService is null");

			int postsCapacity = SitecoreHelper.GetSiteSettingItem(sitecoreService).PostsPageCapacity;

			_searchEngineService = searchEngineService;
			_sitecoreService = sitecoreService;
			_postsCapacity = postsCapacity;
		}

		public FreshPostsResult GetFreshPostsSinceDate(string indexName, DateTime date)
		{
			var searchResult = _searchEngineService.SearchPosts(indexName, date);
			var postItemCollection = new List<PostItemModel>();

			if (searchResult.Count() == 0)
			{
				return new FreshPostsResult()
				{
					IsSuccesful = false
				};
			}

			foreach (var result in searchResult)
			{
				var postItem = ConvertSearchResultModelToPostItemModel(result);
				postItemCollection.Add(postItem);
			}

			return new FreshPostsResult()
			{
				IsSuccesful = true,
				SearchResult = postItemCollection
			};

		}

		public SearchModel SearchPosts(string indexName, string title, int page, IList<ID> tags, IList<ID> categories)
		{
			var searchResult = _searchEngineService.SearchPosts(indexName, title, page, _postsCapacity, tags, categories);
			var siteSettingItem = SitecoreHelper.GetSiteSettingItem(_sitecoreService);

			if (searchResult.SearchResut.Count() == 0)
			{
				return new SearchModel()
				{
					IsSuccessful = false,
					NoResultMessage = siteSettingItem.NoResultMessage
				};
			}

			var postCollection = new List<PostItemModel>();

			foreach (var result in searchResult.SearchResut)
			{
				var postModel = ConvertSearchResultModelToPostItemModel(result);
				postCollection.Add(postModel);
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
			var sitecoreService = new SitecoreService("master");
			var searchPostsQuerry = $"{contextItem.Paths.FullPath}/*[contains(@title,'{title}') and @@templatekey='post']";

			var searchResultItems = Sitecore.Context.Database.SelectItems(searchPostsQuerry);

			if (searchResultItems != null && searchResultItems.Length > 0)
			{
				var postCollection = new List<PostItemModel>();

				foreach (var item in searchResultItems)
				{
					sitecoreService.Map(item);
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

		private PostItemModel ConvertSearchResultModelToPostItemModel(SearchHit<PostSearchIndexModel> searchResultItem)
		{
			var resultItem = searchResultItem.Document.GetItem();
			MultilistField postTagsField = resultItem.Fields["Tags"];
			LookupField category = resultItem.Fields["Category"];
			var options = LinkManager.GetDefaultUrlOptions();
			options.AlwaysIncludeServerUrl = true;
			options.SiteResolving = true;

			var post = new PostItemModel()
			{
				Body = resultItem.Fields["Body"].Value,
				Subtitle = resultItem.Fields["Subtitle"].Value,
				Title = resultItem.Fields["Title"].Value,
				Url = LinkManager.GetItemUrl(resultItem, options),
				Author = resultItem.Fields["Author"].Value,
				Date = (new DateField(resultItem.Fields["Date"])).DateTime,
				Tags = postTagsField.Count != 0 ? postTagsField.GetItems().Select(i => i.Fields["Value"].Value) : new List<string>(),
				Category = category.TargetItem != null ? category.TargetItem.Fields["Value"].Value : null,
			};

			return post;
		}
	}
}
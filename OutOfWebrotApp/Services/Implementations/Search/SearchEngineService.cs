using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using OutOfWebrotApp.Helpers;
using OutOfWebrotApp.Models.Components.SearchIndex;
using OutOfWebrotApp.Models.Services.SearchEngineService;
using OutOfWebrotApp.Services.Interfaces.Search;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Mvc.Extensions;

namespace OutOfWebrotApp.Services.Implementations.Search
{
	public class SearchEngineService : ISearchEngineService
	{
		//private readonly string _indexId;

		//public SearchEngineService()
		//{
		//	var item = SitecoreHelper.GetSiteSettingItem();
		//	_indexId = item.Fields["PostIndex"].Value;
		//}

		public SearchEngineSearchResult SearchPosts(string title, int page, int pageCapacity,IList<ID> tags, IList<ID> categories)
		{
			var item = SitecoreHelper.GetSiteSettingItem();
			var indexId = item.Fields["PostIndex"].Value;

			var index = ContentSearchManager.GetIndex(indexId);
			var postTags = tags == null ? new List<ID>() : tags;
			var postCategories = categories == null ? new List<ID>() : categories;
			var postTitle = title.IsEmptyOrNull() ? null : title;

			var categoriesPredicateList = new List<Expression<Func<PostSearchIndexModel, bool>>>();

			using (var context = index.CreateSearchContext())
			{

				if (postCategories.Any())
				{
					var resultPredicate = BuildPredicateForCategorySubstringAndTags(postTags, postCategories, postTitle);
					categoriesPredicateList = resultPredicate;
				}
				else
				{
					var truePredicate = PredicateBuilder.True<PostSearchIndexModel>();
					var predicate = BuildPredicateForSubstringAndTags(truePredicate, postTags, postTitle);
					categoriesPredicateList.Add(predicate);
				}

				var currentPredicate = PredicateBuilder.False<PostSearchIndexModel>();

				foreach (var predicate in categoriesPredicateList)
				{
					currentPredicate = currentPredicate.Or(predicate);
				}

				var result = context.GetQueryable<PostSearchIndexModel>()
					.Where(currentPredicate)
					.Filter(c => c.Language == Sitecore.Context.Language.Name)
					.FacetOn(c => c.Category)
					.FacetOn(c => c.PostTagsString)
					.Page(page - 1, pageCapacity).GetResults();

				return new SearchEngineSearchResult()
				{
					CurrentPage = page,
					PageCapacity = pageCapacity,
					SearchResut = result.ToList(),
					TotalPageAmount = (int)Math.Ceiling(result.TotalSearchResults / (double)pageCapacity),
					TotalPostAmount = result.TotalSearchResults
				};
			}
		}

		private Expression<Func<PostSearchIndexModel, bool>> BuildPredicateForTags(
			Expression<Func<PostSearchIndexModel, bool>> predicateBuilder, IEnumerable<ID> tags)
		{
			var predicate = predicateBuilder;

			foreach (var tag in tags)
			{
				predicate = predicate.And(c => c.Tags.Contains(tag));
			}

			return predicate;
		}

		private Expression<Func<PostSearchIndexModel, bool>> BuildPredicateForSubstringAndTags(
			Expression<Func<PostSearchIndexModel, bool>> predicateBuilder, IEnumerable<ID> tags, string substring)
		{
			var predicate = predicateBuilder;


			if (substring != null)
			{
				predicate = predicate.And(c =>
					c.Title.Contains(substring) || c.Subtitle.Contains(substring) || c.PostTagsString.Contains(substring) || c.Body.Contains(substring));
			}

			return BuildPredicateForTags(predicate, tags);
		}

		private List<Expression<Func<PostSearchIndexModel, bool>>> BuildPredicateForCategorySubstringAndTags(IEnumerable<ID> tags, IEnumerable<ID> categories,
			string substring)
		{
			var categoriesPredicateList = new List<Expression<Func<PostSearchIndexModel, bool>>>();

			foreach (var category in categories)
			{
				var predicateBuilder = PredicateBuilder.True<PostSearchIndexModel>();
				predicateBuilder = predicateBuilder.And(c => c.Category == category);
				if (substring != null)
				{
					predicateBuilder = predicateBuilder.And(c =>
						c.Title.Contains(substring) || c.Subtitle.Contains(substring) || c.PostTagsString.Contains(substring) || c.Body.Contains(substring));
				}

				predicateBuilder = BuildPredicateForTags(predicateBuilder, tags);

				categoriesPredicateList.Add(predicateBuilder);
			}

			return categoriesPredicateList;
		}
	}
}
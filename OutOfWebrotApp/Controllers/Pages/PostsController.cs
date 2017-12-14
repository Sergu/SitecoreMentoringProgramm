using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InfrastructureModule.Helpers;
using InfrastructureModule.Models.Components.TagsTree;
using InfrastructureModule.Models.Pages.Search;
using InfrastructureModule.Services.Interfaces.Search;
using OutOfWebrotApp.Scheduling;
using Sitecore.Data;

namespace OutOfWebrotApp.Controllers.Pages
{
    public class PostsController : Controller
    {
	    private readonly ISearchService _searchService;

	    public PostsController(ISearchService searchService)
	    {
		    _searchService = searchService;
	    }
        // GET: Posts
        public ActionResult Index()
        {
	        string subString = null;
	        var url = Request.Url;
	        string param = HttpUtility.ParseQueryString(url.Query).Get("s");
	        if (param != null)
	        {
		        subString = param;
	        }

			var item = SitecoreHelper.GetSiteSettingItem();
			var indexName = item.Fields["PostIndex"].Value;

			var searchResult = _searchService.SearchPosts(indexName, subString, 1, new List<ID>(), new List<ID>());

	        if (searchResult == null)
	        {
		        if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
		        {
			        return View("~/Views/Errors/ExperienceEditorError.cshtml");
		        }

				return View("~/Views/Pages/Posts/Posts.cshtml", new SearchModel());
			}

	        var postsStateTracker = new PostsStateTracker(_searchService);

	        postsStateTracker.FreshPostsDispatchingByEmail();


			return View("~/Views/Pages/Posts/Posts.cshtml", searchResult);
        }

		[HttpGet]
	    public ActionResult SubstringPostSearch()
		{
			return View("~/Views/Components/SubstringPostSearch/SubstringPostSearch.cshtml");
		}

	    [HttpGet]
	    public ActionResult TagsTree()
	    {
		    var postTagTreeRootItem = SitecoreHelper.GetPostTagtreeRootItem();
		    var tagsTreeJson = System.Web.Helpers.Json.Encode(TagsTreeHelper.GetTagsTree(postTagTreeRootItem));

		    var tagsTreeModel = new TagsTreeModel()
		    {
				JsonTagsTree = tagsTreeJson
		    };


			return View("~/Views/Components/TagsTree/TagsTree.cshtml", tagsTreeModel);
	    }

		[HttpGet]
	    public ActionResult PostsCategories()
		{

			var postCategoryRootItem = SitecoreHelper.GetPostCategoryrootItemId();
		    var categoryTreeJson = System.Web.Helpers.Json.Encode(TagsTreeHelper.GetTagsTree(postCategoryRootItem));
		    var categoryTreeModel = new CategoryTreeModel()
		    {
			    JsonCategoryTree = categoryTreeJson
		    };

			return View("~/Views/Components/CategoryTree/CategoryTree.cshtml", categoryTreeModel);
		}

	    public JsonResult GetPostsPartialView(string title, List<string> tags, List<string> categories, int page)
	    {
			var postTags = tags == null ? new List<ID>() : tags.Select(t => new ID(t)).ToList();
			var postcategories = categories == null ? new List<ID>() : categories.Select(c => new ID(c)).ToList();
			var currentPage = page;
			var substring = title;
			var item = SitecoreHelper.GetSiteSettingItem();
			var indexName = item.Fields["PostIndex"].Value;

			var searchResult = _searchService.SearchPosts(indexName, substring, currentPage, postTags, postcategories);

			var renderResult = RenderRazorViewToString("~/Views/Pages/Posts/PartialPostsView.cshtml", searchResult);

		    return Json(renderResult, JsonRequestBehavior.AllowGet);
	    }

	    public string RenderRazorViewToString(string viewName, object model)
	    {
		    ViewData.Model = model;
		    using (var sw = new StringWriter())
		    {
			    var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
				    viewName);
			    var viewContext = new ViewContext(ControllerContext, viewResult.View,
				    ViewData, TempData, sw);
			    viewResult.View.Render(viewContext, sw);
			    viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
			    return sw.GetStringBuilder().ToString();
		    }
	    }
	}
}
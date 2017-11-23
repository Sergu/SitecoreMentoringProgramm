﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OutOfWebrotApp.Helpers;
using OutOfWebrotApp.Models.Components.TagsTree;
using OutOfWebrotApp.Models.Pages.Post;
using OutOfWebrotApp.Models.Pages.Search;
using OutOfWebrotApp.Services.Interfaces.Search;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using Sitecore.Pipelines.Rules.Taxonomy;
using Sitecore.Shell.Feeds.FeedTypes;
using Sitecore.StringExtensions;
using Sitecore.Web.UI.XslControls;

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

	        var it = Sitecore.Context.Item;

	        var searchResult = _searchService.SearchPosts(subString, 1, new List<ID>(), new List<ID>());

	        if (searchResult == null)
	        {
		        if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
		        {
			        return View("~/Views/Errors/ExperienceEditorError.cshtml");
		        }

				return View("~/Views/Pages/Posts/Posts.cshtml", new SearchModel());
			}

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
		    var tagsTreeJson = TagsTreeHelper.GetTagsTreeJson(postTagTreeRootItem);

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
		    var categoryTreeJson = TagsTreeHelper.GetTagsTreeJson(postCategoryRootItem);
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

			var searchResult = _searchService.SearchPosts(substring, currentPage, postTags, postcategories);

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
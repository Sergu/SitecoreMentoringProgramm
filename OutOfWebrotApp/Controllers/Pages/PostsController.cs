using System;
using System.Collections.Generic;
using System.Web.Mvc;
using OutOfWebrotApp.Models.Pages.Post;
using OutOfWebrotApp.Services.Interfaces.Search;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

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
	        //var searchresultCount = _searchService.GetSearchResultNumber("to");

			var posts = new Models.Pages.Posts.Posts();
			var postCollection = new List<PostItemModel>();
	        var siteContext = RenderingContext.Current.Rendering.Item.Children;

	        if (siteContext == null)
	        {
		        if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
		        {
			        return View("~/Views/Errors/ExperienceEditorError.cshtml");
		        }

				return View("~/Views/Pages/Posts/Posts.cshtml", posts);
			}

			foreach (Item child in siteContext)
	        {
				var dateField = child.Fields["Date"].HasValue ? new DateField(child.Fields["Date"]).DateTime : DateTime.Now;


		        var post = new PostItemModel()
		        {
			        Body = child.Fields["Body"].HasValue ? child.Fields["Body"].Value : string.Empty,
			        Subtitle = child.Fields["Subtitle"].HasValue ? child.Fields["Subtitle"].Value : string.Empty,
			        Title = child.Fields["Title"].HasValue ? child.Fields["Title"].Value : string.Empty,
			        Url = LinkManager.GetItemUrl(child),
			        Author = child.Fields["Author"].HasValue ? child.Fields["Author"].Value : string.Empty,
			        Date = dateField
		        };

		        var titleFieldId = child.Fields["Title"].ID;
		        var subtitleFieldId = child.Fields["Subtitle"].ID;

				postCollection.Add(post);
	        }
	        posts.PostsCollection = postCollection;

			return View("~/Views/Pages/Posts/Posts.cshtml", posts);
        }

	    [HttpGet]
	    public ActionResult TagsTree()
	    {
		    var res = Server.MapPath("~/Content/Images/TagsTree/imgs/");
		    var urlContent = Url.Content(res);

			return View("~/Views/Components/TagsTree/TagsTree.cshtml");
	    }
	}
}
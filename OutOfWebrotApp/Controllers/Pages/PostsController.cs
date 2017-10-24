using System;
using System.Collections.Generic;
using System.Web.Mvc;
using OutOfWebrotApp.Models.Pages.Post;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace OutOfWebrotApp.Controllers.Pages
{
    public class PostsController : Controller
    {
        // GET: Posts
        public ActionResult Index()
        {
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
				postCollection.Add(post);
	        }
	        posts.PostsCollection = postCollection;

			return View("~/Views/Pages/Posts/Posts.cshtml", posts);
        }
    }
}
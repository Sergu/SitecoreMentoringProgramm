using System.Collections.Generic;
using System.Web.Mvc;
using OutOfWebrotApp.Models.Pages.Post;
using Sitecore.Data.Fields;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace OutOfWebrotApp.Controllers.Pages
{
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index()
        {
	        var contextItem = RenderingContext.Current.Rendering.Item;

	        if (contextItem == null)
	        {
		        if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
		        {
			        return View("~/Views/Errors/ExperienceEditorError.cshtml");
		        }

				return View("~/Views/Pages/Post/Post.cshtml", new PostItemModel());
			}

	        var commentSearchQuerry = $"{contextItem.Paths.FullPath}//*[@@templatekey='comment']";

	        var commentItems = Sitecore.Context.Database.SelectItems(commentSearchQuerry);
	        var comments = new List<Comment>();

	        foreach (var commentItem in commentItems)
	        {
		        DateField date = commentItem.Fields["Date"];
		        var comment = new Comment()
		        {
			        Author = commentItem.Fields["Author"].Value,
			        Date = date.DateTime,
			        Text = commentItem.Fields["Text"].Value
		        };
				comments.Add(comment);
	        }

			var post = new PostItemModel()
	        {
		        Body = contextItem.Fields["Body"].Value,
		        Subtitle = contextItem.Fields["Subtitle"].Value,
		        Title = contextItem.Fields["Title"].Value,
		        Url = LinkManager.GetItemUrl(contextItem),
		        Author = contextItem.Fields["Author"].Value,
		        Date = (new DateField(contextItem.Fields["Date"])).DateTime,
				Comments = comments
	        };


	        return View("~/Views/Pages/Post/Post.cshtml", post);
        }
    }
}
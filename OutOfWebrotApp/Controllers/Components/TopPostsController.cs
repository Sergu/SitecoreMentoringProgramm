using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glass.Mapper.Sc.Web.Mvc;
using InfrastructureModule.Helpers;
using InfrastructureModule.Models.Pages.Post;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace OutOfWebrotApp.Controllers.Components
{
	public class TopPostsController : GlassController
	{
		// GET: TopPosts
		public override ActionResult Index()
		{
			var postCollection = new List<PostItemModel>();
			var contextItem = SitecoreHelper.GetPostsItem();

			if (contextItem == null)
			{
				if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
				{
					return View("~/Views/Errors/ExperienceEditorError.cshtml");
				}

				return View("~/Views/Components/TopPosts/TopPosts.cshtml", postCollection);
			}

			var postsItems = contextItem.Children.ToList().Take(3).ToList();
			foreach (Item child in postsItems)
			{

				var dateField = child.Fields["Date"].HasValue ? new DateField(child.Fields["Date"]).DateTime : DateTime.Now;


				var post = new PostItemModel();

				post.Body = child.Fields["Body"].HasValue ? child.Fields["Body"].Value : String.Empty;
				post.Subtitle = child.Fields["Subtitle"].HasValue ? child.Fields["Subtitle"].Value : String.Empty;
				post.Title = child.Fields["Title"].HasValue ? child.Fields["Title"].Value : String.Empty;
				post.Url = LinkManager.GetItemUrl(child);
				post.Author = child.Fields["Author"].HasValue ? child.Fields["Author"].Value : String.Empty;
				post.Date = dateField;

				postCollection.Add(post);
			}

			return View("~/Views/Components/TopPosts/TopPosts.cshtml", postCollection);
		}
	}
}
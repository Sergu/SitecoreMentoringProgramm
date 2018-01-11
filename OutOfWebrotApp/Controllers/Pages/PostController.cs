using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glass.Mapper.Sc.Web.Mvc;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using InfrastructureModule.Helpers;
using InfrastructureModule.Models.Pages.Post;
using InfrastructureModule.Services.Interfaces.Publishing;
using InfrastructureModule.TDS.sitecore.templates.Custom.BaseTemplates.Components.Comment.Base;
using InfrastructureModule.TDS.sitecore.templates.Custom.BaseTemplates.Pages.Post;
using Comment = InfrastructureModule.Models.Pages.Post.Comment;

namespace OutOfWebrotApp.Controllers.Pages
{
    public class PostController : GlassController
    {
	    private readonly IPublishingService _publishService;

	    public PostController(IPublishingService publishService)
	    {
		    _publishService = publishService;
	    }
        // GET: Post
        public override ActionResult Index()
        {
	        var sitecoreService = this.SitecoreContext;
	        Item contextItem = GetContextItem<Item>();
	        IPost contextPostItem = GetContextItem<IPost>();

	        if (contextPostItem == null)
	        {
		        if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
		        {
			        return View("~/Views/Errors/ExperienceEditorError.cshtml");
		        }

		        return View("~/Views/Pages/Post/Post.cshtml", new PostItemModel());
	        }

	        var postTagsField = contextPostItem.Tags.ToList();
	        var category = contextPostItem.Category;

			var post = new PostItemModel()
	        {
		        Body = contextPostItem.Body,
		        Subtitle = contextPostItem.Subtitle,
		        Title = contextPostItem.Title,
		        Url = LinkManager.GetItemUrl(contextItem),
		        Author = contextPostItem.Author,
		        Date = contextPostItem.Date,
				Tags = postTagsField.Count != 0 ? postTagsField.Select(id => id.ToString()).ToList() : new List<string>(),
				Category = category != Guid.Empty ? category.ToString() : null,
	        };

	        var layoutGuid = RenderingContext.CurrentOrNull.Rendering.LayoutId;
	        var layoutItem = Sitecore.Context.Database.GetItem(new ID(layoutGuid));
	        var layoutName = layoutItem.Name;

	        if (layoutName == "PrintLayout")
	        {
		        return View("~/Views/Pages/Post/PostPrintView.cshtml", post);
			}

	        return View("~/Views/Pages/Post/Post.cshtml", post);
        }

	    public ActionResult Comments()
	    {
		    var sitecoreService = this.SitecoreContext;
			var contextItem = GetContextItem<Item>();

		    if (contextItem == null)
		    {
			    if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
			    {
				    return View("~/Views/Errors/ExperienceEditorError.cshtml");
			    }

			    return View("~/Views/Pages/Post/Post.cshtml", new PostItemModel());
		    }

			var commentSearchQuerry = $"fast:{contextItem.Paths.FullPath}//*[@@templatekey='comment']";

		    var commentSearchResult = Sitecore.Context.Database.SelectItems(commentSearchQuerry).ToList();
		    var commentItems = commentSearchResult.Select(r => sitecoreService.GetItem<IComment>(r.ID.Guid)).ToList();
		    var comments = new List<Comment>();

		    foreach (var commentItem in commentItems)
		    {
			    var comment = new Comment()
			    {
				    Author = commentItem.Author,
				    Date = commentItem.Date,
				    Text = commentItem.Text
			    };
			    comments.Add(comment);
		    }

			var orderedComments = comments.OrderByDescending(c => c.Date);

		    return View("~/Views/Components/Comments/Comment.cshtml", orderedComments);
	    }

		[HttpGet]
	    public ActionResult GetCommentPostForm()
	    {
			return View("~/Views/Components/Comments/CommentPostForm.cshtml");
		}

	    [HttpPost]
		[ValidateInput(false)]
	    public ActionResult GetCommentPostForm(Comment comment)
	    {
		    var sitecoreService = this.SitecoreContext;
		    var contextItem = GetContextItem<Item>();

			//Database masterDatabase = Sitecore.Data.Database.GetDatabase("master");
			//var masterContextItem = masterDatabase.GetItem(contextItem.ID);
			//var templatePath = SitecoreHelper.GetSiteSettingItem(sitecoreService).CommentTemplateId;
			//var templateItem = masterDatabase.GetTemplate(new ID(templatePath));

		    var freshComment = new Comment()
		    {
			    Author = comment.Author,
			    Date = DateTime.Now,
			    Text = comment.Text
		    };

		    sitecoreService.Create(contextItem, freshComment);

			//using (new Sitecore.SecurityModel.SecurityDisabler())
		 //   {
			//	var commentItem = masterContextItem.Add("Comment", new TemplateID(templateItem.ID));
			//    if (commentItem != null)
			//    {
			//	    commentItem.Editing.BeginEdit();
			//	    commentItem["Text"] = comment.Text;
			//	    commentItem["Author"] = "John Black";
			//	    commentItem["Date"] = DateUtil.ToIsoDate(DateTime.Now);
			//	    commentItem.Editing.EndEdit();

			//		_publishService.PublishItemToWebDatabase(commentItem, false);
			//    }
			//}

		    return Redirect(LinkManager.GetItemUrl(contextItem));
	    }

	    public ActionResult CheckContextItemOnNull(Item item)
	    {
			if (item == null)
			{
				if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
				{
					return View("~/Views/Errors/ExperienceEditorError.cshtml");
				}

				return View("~/Views/Pages/Post/Post.cshtml", new PostItemModel());
			}

		    return null;
	    }

	}
}